using System;
using System.Collections.Generic;
using FormForge.AssetManagement;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.Core.Services;
using FormForge.Infrastructure.UI.Screens.Messages;
using FormForge.Infrastructure.UI.Screens.Model;
using FormForge.Infrastructure.UI.Screens.View;
using FormForge.Messaging.Interfaces;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace FormForge.Infrastructure.UI.Screens
{
	public class ScreenManager : IMessageReceiver<OpenScreenMessage>,
		IMessageReceiver<CloseScreenMessage>
    {
	    private Transform m_ScreenContainer;
		private ScreenPresenter m_CurrentScreen;

		private Dictionary<Type, ScreenPresenter> m_Screens = new Dictionary<Type, ScreenPresenter>();

		private DateTime m_OpenTimestamp;

		public ScreenManager(Transform screenContainer)
		{
			m_ScreenContainer = screenContainer;
			
			var messagingService = ServiceLocator.GetService<IMessageService>();
			messagingService.Register<OpenScreenMessage>(this);
			messagingService.Register<CloseScreenMessage>(this);
		}
        
		~ScreenManager()
		{
			var messagingService = ServiceLocator.GetService<IMessageService>();
			messagingService.Unregister<OpenScreenMessage>(this);
			messagingService.Unregister<CloseScreenMessage>(this);
		}

		public void HandleMessage(OpenScreenMessage messageData = null)
		{
			OpenScreen(messageData.ViewModel);
		}

		public void HandleMessage(CloseScreenMessage messageData = null)
		{
			CloseScreen(messageData.ScreenType);
		}

		/// <summary>
		/// Open a new screen. Reuse it if its already instantiated, or create a new instance otherwise.
		/// </summary>
		private async void OpenScreen<TScreenViewModel>(TScreenViewModel viewModel) 
			where TScreenViewModel : IScreenViewModel
		{
			Type screenType = typeof(TScreenViewModel);
			Debug.Log($"Requesting opening {screenType.Name}...");

			m_OpenTimestamp = DateTime.UtcNow;

			// Get the prefab address via reflection
			string address = (string)screenType.GetField("s_address")?.GetValue(null);
			if (address == null)
			{
				Debug.LogError($"The screen {screenType} doesn't define s_address");
				return;
			}

			// Check if the screen is already instantiated
			if (m_Screens.ContainsKey(screenType))
			{
				ScreenPresenter screenToOpen = m_Screens[screenType];
				bool focusScreen = false;

				// Close before open
				if (screenToOpen.CloseOtherScreensOnOpen)
				{
					CloseOtherScreens(screenToOpen);
				}

				// Configure the new screen before opening
				await screenToOpen.Configure(viewModel);

				// Open the screen - different actions based on its current state
				if (screenToOpen.ScreenState == State.Foreground)
				{
					// Do nothing
				}
				else if (screenToOpen.ScreenState == State.Background)
				{
					focusScreen = true;
				}
				else if (screenToOpen.ScreenState == State.Closed)
				{
					screenToOpen.gameObject.SetActive(true);
					InternalOpen(screenToOpen);
					focusScreen = true;
				}

				// Focus the screen last. Order is important.
				if (focusScreen)
				{
					FocusScreen(screenToOpen);
				}
			}
			else
			{
				GameObject prefab = await ServiceLocator.GetService<IAssetManagementService>().
					LoadAsync<GameObject, UIContext>(new BasicAssetPolicy(address));

				ScreenPresenter presenter = prefab.GetComponent<ScreenPresenter>();
				if (presenter == null)
				{
					Debug.LogError($"The screen doesn't contain a Screen component");
				}
				
				ScreenPresenter loadedScreen = Object.Instantiate(presenter, m_ScreenContainer, false);

				// Add it to current screens collection
				m_Screens.Add(screenType, loadedScreen);

				// Initialize the screen
				await loadedScreen.Initialize();

				if (loadedScreen.CloseOtherScreensOnOpen)
				{
					CloseOtherScreens(loadedScreen);
				}

				if (viewModel != null)
				{
					await loadedScreen.Configure(viewModel);
				}

				InternalOpen(loadedScreen);

				// Open the loaded screen
				FocusScreen(loadedScreen);
			}
		}

		/// <summary>
		/// Disables/destroy the target screen.
		/// If closing the current screen, take the next one in the hierarchy as current screen.
		/// </summary>
		private void CloseScreen(Type type)
		{
			// Make sure the screen exists
			if (!m_Screens.ContainsKey(type))
			{
				return;
			}
			
			var targetScreen = m_Screens[type];
			if (targetScreen.ScreenState == State.Closed)
			{
				// Already closed
				return;
			}

			// Perform the close
			InternalClose(targetScreen);
		}

		/// <summary>
		/// Close all the active screens
		/// </summary>
		public void CloseAllScreens()
		{
			foreach (ScreenPresenter screen in m_Screens.Values)
			{
				if (screen != null && screen.isActiveAndEnabled)
				{
					// Use close internal, so we skip the logic of opening the screen below
					screen.CloseInternal();
				}
			}
		}

		public void DestroyClosedScreens()
		{
			List<ScreenPresenter> screenList = new List<ScreenPresenter>(m_Screens.Values);
			foreach (ScreenPresenter screen in screenList)
			{
				if (screen != null && screen.ScreenState == State.Closed)
				{
					DestroyScreen(screen);
				}
			}
		}

		/// <summary>
		/// Internal operations for Opening a screen
		/// </summary>
		/// <param name="screen"></param>
		private void InternalOpen(ScreenPresenter screen)
		{
			// Call the open screen code
			screen.Open();

			// Notify
			//m_MessengerService.Broadcast(MessengerEvents.ScreenOpened, screen);
		}

		/// <summary>
		/// Internal operations for closing a screen
		/// </summary>
		/// <param name="screen"></param>
		private void InternalClose(ScreenPresenter screen)
		{
			if (screen.KeepScreenOpened)
			{
				return;
			}

			if (screen.ScreenState == State.Foreground)
			{
				screen.LoseFocus();
			}

			// Call the close event
			screen.CloseInternal();

			// Disable the game object
			screen.gameObject.SetActive(false);

			// Notify
			//m_MessengerService.Broadcast(MessengerEvents.ScreenClosed, screen);

			// Destroy when closed?
			if (screen.DestroyOnClose)
			{
				DestroyScreen(screen);
			}
		}

		/// <summary>
		/// Process a screen that has been focused
		/// Add it to history and process all the focus/closing stuff.
		/// </summary>
		/// <param name="newScreen">The screen that has been opened/activated</param>
		private void FocusScreen(ScreenPresenter newScreen)
		{
			// The current screen loses the focus
			if (m_CurrentScreen != null)
			{
				m_CurrentScreen.LoseFocus();
			}

			// While the new one receives it
			// The screen will be moved to the top in the late update phase
			newScreen.GetFocus();

			ScreenPresenter previousScreen = m_CurrentScreen;
			m_CurrentScreen = newScreen;

			string prev = previousScreen == null ? "none" : previousScreen.GetType().Name;
			Debug.Log($"Changed focus: {prev} -> {m_CurrentScreen.GetType().Name}");

			// Log opening time for benchmark
			TimeSpan openingDuration = DateTime.UtcNow - m_OpenTimestamp;
			Debug.Log($"Screen {newScreen.name} opened in {openingDuration.TotalMilliseconds} ms");

			// Show a warning in console if loading time is too much
			if (openingDuration.TotalMilliseconds >= 100)
			{
				Debug.LogWarning($"Screen {newScreen.name} opened in {openingDuration.TotalMilliseconds} ms!" +
				                 " Try to use preload or lighten up the screen prefab.");
			}
		}

		/// <summary>
		/// Destroy an existing screen. No safe checks made.
		/// </summary>
		private void DestroyScreen(ScreenPresenter screen)
		{
			// Remove the screen from the dictionary
			m_Screens.Remove(screen.ViewModel.GetType());

			// Call the event in case this screen needs to run some pre-destruction code
			screen.Dispose();

			Object.Destroy(screen.gameObject);
		}

		/// <summary>
		/// Close all screens except the current one
		/// </summary>
		/// <param name="currentScreen"></param>
		private void CloseOtherScreens(ScreenPresenter currentScreen)
		{
			List<Type> screensToClose = GetOtherActiveScreens(currentScreen);
			CloseScreens(screensToClose);
		}

		/// <summary>
		/// Get all screens that are in foreground or background except the current one
		/// </summary>
		/// <param name="currentScreen"></param>
		private List<Type> GetOtherActiveScreens(ScreenPresenter currentScreen)
		{
			List<Type> otherScreens = new List<Type>();
			foreach (Type key in m_Screens.Keys)
			{
				// Ignore closed screens
				if (m_Screens[key].ScreenState == State.Closed)
				{
					continue;
				}

				if (key != currentScreen.ViewModel.GetType())
				{
					// Cannot modify the dictionary while iterating
					otherScreens.Add(key);
				}
			}
			return otherScreens;
		}

		/// <summary>
		/// Close several screens in a batch
		/// </summary>
		/// <param name="screensToClose">List of screens to close</param>
		private void CloseScreens(List<Type> screensToClose)
		{
			foreach (Type screenToClose in screensToClose)
			{
				// Don't restore screens to avoid infinite loops
				CloseScreen(screenToClose);
			}
		}
    }
}

