using System.Threading.Tasks;
using FormForge.Core.Services;
using FormForge.Infrastructure.UI.Screens.Messages;
using FormForge.Infrastructure.UI.Screens.Model;
using FormForge.Messaging.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.Infrastructure.UI.Screens.View
{
	public enum State
	{
		None,       // Not initialized yet
		Foreground, // Screen is open and focused
		Background, // Screen is open but not focused
		Closed,      // Screen is created but disabled,
	}
	
    public class ScreenPresenter : MonoBehaviour, IScreenPresenter<IScreenViewModel>
    {
	    [Tooltip("If checked: this screen will be destroyed when closed." +
	             " Uncheck it if we intend to visit it again later.")]
        [SerializeField] protected bool m_DestroyOnClose;
	    public bool DestroyOnClose => m_DestroyOnClose;
        
        [Tooltip("If checked: won't close the screen when another one is opened")]
        [SerializeField] protected bool m_KeepScreenOpened;
        public bool KeepScreenOpened => m_KeepScreenOpened;
        
        [Tooltip("If checked: will close all the other screens when this one is open")]
        [SerializeField] protected bool m_CloseOtherScreensOnOpen = true;
        public bool CloseOtherScreensOnOpen => m_CloseOtherScreensOnOpen;
        
        [SerializeField] protected Button m_BackgroundOverlay;

        public IScreenViewModel ViewModel
        {
	        get;
	        set;
        }

        public string ScreenId => GetType().Name;

        /// <summary>
        /// Current state of the screen.
        /// </summary>
        public State ScreenState => GetState();
        
	    /// <summary>
		/// Is the screen initialized?<br/>
		/// The screen is considered initialized once its prefab has been loaded and instantiated, 
		/// but it may not have been configured yet with the latest parameters. 
		/// See <see cref="IsConfigured"/> for that.
		/// </summary>
		public virtual bool IsInitialized => m_IsInitialized;
	    private bool m_IsInitialized;

		/// <summary>
		/// Is the screen configured?<br/>
		/// The screen is considered configured when their parameters have been processed and applied.
		/// </summary>
		public bool IsConfigured => m_IsConfigured;
		private bool m_IsConfigured;

		/// <summary>
		/// Is the screen focused?<br/>
		/// The screen is considered focused when its the one in the foreground (the "current" screen).
		/// Only one screen can have the focus at a given time.
		/// </summary>
		public bool IsFocused => m_IsFocused;
		private bool m_IsFocused;

		/// <summary>
		/// Is the screen open?<br/>
		/// The screen is considered open when it is active on screen, regardless of whether it has the focus or not.
		/// </summary>
		public bool IsOpen => ScreenState == State.Background || ScreenState == State.Foreground;

		// Internal logic
		// Use this flag to avoid several refreshes in the same frame
		private bool m_NeedsRefresh;

		// This flag moves the screen to the top of the hierarchy AFTER the refresh, 
		// so we hide flashing the outdated screen for one frame 
		private bool m_NeedsToMoveToTop;

		// This flag remember if we have blocked the UI, so we dont destroy the screen leaving it blocked
		private bool m_HasBlockedUI;

		protected virtual void LateUpdate()
		{
			if (m_NeedsRefresh)
			{
				DoRefresh();
			}
			if (m_NeedsToMoveToTop)
			{
				transform.SetAsLastSibling();
				m_NeedsToMoveToTop = false;
			}
		}

		/// <summary>
		/// Close the screen (but not necessarily destroying it,
		/// could be only deactivated and kept in a second plane)
		/// </summary>
		public virtual void Close()
		{
			ServiceLocator.GetService<IMessageService>().Send(new CloseScreenMessage(typeof(IScreenViewModel)));
		}

		/// <summary>
		/// The screen has been just created. This is called once in the screen lifetime.
		/// </summary>
		public virtual async Task Initialize()
		{
			Debug.Log($"Initialize screen \"{GetType().Name}\"");
			if (!m_IsInitialized)
			{
				m_IsInitialized = true;
				OnInitialize();
			}
		}

		/// <summary>
		/// Configure the screen with the given parameters
		/// </summary>
		/// <param name="viewModel"></param>
		public virtual async Task Configure(IScreenViewModel viewModel)
		{
			Debug.Log($"Configure With Params screen \"{GetType().Name}\"");
			m_IsConfigured = true;
			ViewModel = viewModel;

			OnConfigure(viewModel);
		}
		
		/// <summary>
		/// The screen will open.
		/// </summary>
		public void Open()
		{
			Debug.Log($"Open screen \"{GetType().Name}\"");
			OnOpen();
		}

		/// <summary>
		/// The screen has received the focus.
		/// </summary>
		public void GetFocus()
		{
			Debug.Log($"Get Focus screen \"{GetType().Name}\"");
			m_IsFocused = true;

			// Invoke internal message
			OnGetFocus();

			// By default redraw the UI when a screen gets the focus back
			m_NeedsRefresh = true;
			m_NeedsToMoveToTop = true;

			// Notify
			//messengerService.Broadcast(MessengerEvents.ScreenGotFocus, this);
		}

		/// <summary>
		/// Request refreshing the screen's state.<br/>
		/// The refresh happens when the screen gets focus, or when manually requested, 
		/// and always at the end of the frame.
		/// </summary>
		public void Refresh()
		{
			// Will refresh at the end of the update phase, before rendering
			m_NeedsRefresh = true;
		}

		/// <summary>
		/// The screen has lost focus.
		/// </summary>
		public void LoseFocus()
		{
			Debug.Log($"Lose Focus screen \"{GetType().Name}\"");
			m_IsFocused = false;

			// Invoke internal message
			OnLoseFocus();

			// Notify
			//messengerService.Broadcast(MessengerEvents.ScreenLostFocus, this);
		}

		/// <summary>
		/// Close the screen.
		/// </summary>
		public void CloseInternal()
		{
			Debug.Log($"Close screen \"{GetType().Name}\"");
			
			// Don't leave UI blocked
			if (m_HasBlockedUI)
			{
				//messengerService.Broadcast(MessengerEvents.HideUIBlock);
				m_HasBlockedUI = false;
			}
			
			OnClose();
		}

		/// <summary>
		/// "Destroy" the screen.
		/// </summary>
		public void Dispose()
		{
			Debug.Log($"Dispose screen \"{GetType().Name}\"");
			
			// Don't leave UI blocked
			if (m_HasBlockedUI)
			{
				//messengerService.Broadcast(MessengerEvents.HideUIBlock);
				m_HasBlockedUI = false;
			}
			
			OnDispose();
		}

		/// <summary>
		/// The screen has been created. This will be called only once in the screen lifetime.
		/// </summary>
		protected virtual void OnInitialize()
		{
			
			
		}

		/// <summary>
		/// To be called when the configure method is called.
		/// This method wont be called if the open doesnt have any parameter
		/// </summary>
		/// <param name="viewModel"></param>
		protected virtual void OnConfigure(IScreenViewModel viewModel)
		{
			
			
		}

		/// <summary>
		/// Check if the screen needs to be redirect, so the load can be interrupted
		/// </summary>
		/// <returns>True if the screen is being redirected</returns>
		protected virtual bool CheckForRedirect()
		{
			return false;
		}

		/// <summary>
		/// The screen has been opened.
		/// </summary>
		protected virtual void OnOpen()
		{
			m_NeedsRefresh = true;
		}

		/// <summary>
		/// The screen has received the focus
		/// </summary>
		protected virtual void OnGetFocus() { }

		/// <inheritdoc cref="Refresh"/>
		protected virtual void OnRefresh() { }

		/// <summary>
		/// The screen has passed to a second plane (i.e. behind a popup)
		/// </summary>
		protected virtual void OnLoseFocus() { }

		/// <summary>
		/// The screen has been closed. Not necessarily destroyed (see <see cref="OnDispose"/>).
		/// </summary>
		protected virtual void OnClose() { }

		/// <summary>
		/// The screen has been destroyed. This will be called only once in the screen lifetime.
		/// </summary>
		protected virtual void OnDispose() { }

		/// <summary>
		/// Get the state of the screen based on internal status
		/// </summary>
		/// <returns></returns>
		private State GetState()
		{
			if (!m_IsInitialized)
			{
				return State.None;
			}

			if (!gameObject.activeInHierarchy)
			{
				return State.Closed;
			}

			return m_IsFocused == false ? State.Background : State.Foreground;
		}

		/// <summary>
		/// Perform a screen refresh.
		/// </summary>
		private void DoRefresh()
		{
			// Turn off the flag
			// Important to do it BEFORE the actual refresh logic, in case anything 
			// inside the OnRefresh() call requires a new refresh (which will be performed next frame).
			m_NeedsRefresh = false;
			OnRefresh();
		}

		protected void OnClickBackground()
		{
			m_BackgroundOverlay.gameObject.SetActive(false);
			m_BackgroundOverlay.onClick.RemoveListener(OnClickBackground);
		}
    }
}