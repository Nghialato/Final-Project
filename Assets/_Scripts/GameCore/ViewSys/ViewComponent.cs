using UnityEngine;

namespace _Scripts.GameCore.ViewSys
{
    public class ViewComponent : BaseComponent<ViewData, ViewSystemManager>, IView
    {
        public ViewData viewData;

        public override void UpdateComponent()
        {
            if(viewData.dirty == false) return;
            ViewUpdate();
            viewData.dirty = false;
        }

        public virtual void ViewUpdate()
        {
            
        }
        
        private void OnDisable()
        {
            RemoveFromSystem();
        }
    }
}