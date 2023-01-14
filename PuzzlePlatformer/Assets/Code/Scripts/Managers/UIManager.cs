using DG.Tweening;

namespace Code.Scripts.Managers
{
    public class UIManager : BaseManager<UIManager>
    {
        private void Start()
        {
            var seq = DOTween.Sequence();
            seq.IsActive();
        }
    }
}
