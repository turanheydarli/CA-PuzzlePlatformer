using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Code.Scripts.Managers
{
    public class DialogueManager : BaseManager<DialogueManager>
    {
        [SerializeField] private TMP_Text messageText;

        public void Say(string message, float duration = 2f, float lifetime = 2f)
        {
            DOTween.Sequence()
                .Join(DOTween.To(() => messageText.text, x => messageText.text = x, message, duration))
                .InsertCallback(lifetime + duration, () => messageText.text = string.Empty);
        }
    }
}