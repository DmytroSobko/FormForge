using UnityEngine;

namespace FormForge.Coroutines.YieldInstructions
{
    public class WaitForFrames : CustomYieldInstruction
    {
        private int m_frameCount;
        private int m_targetFrameCount;

        public WaitForFrames(int frames)
        {
            m_targetFrameCount = frames;
            m_frameCount = 0;
        }

        public override bool keepWaiting
        {
            get
            {
                m_frameCount++;
                return m_frameCount <= m_targetFrameCount;
            }
        }

        public new CustomYieldInstruction Reset()
        {
            m_frameCount = 0;
            return this;
        }
    }
}