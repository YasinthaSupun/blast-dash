using UnityEngine;

namespace BlastDash
{
    [CreateAssetMenu(menuName = "Audio Data", order = 2)]
    public class AudioClipsData : ScriptableObject
    {
        // UI
        public AudioClip uiClick;

        //Gameplay
        
        public AudioClip backgroundLobby;
        public AudioClip backgroundGamePlay;
        public AudioClip fire;
        public AudioClip die;
    }
}
