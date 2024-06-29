using UnityEngine;

namespace _Scripts.GameCore.ViewSys
{
    public abstract class ViewComponent : BaseComponent<ViewData, ViewSystemManager>
    {
        public ViewData viewData;

        public override void UpdateComponent()
        {
            if(viewData.dirty == false) return;
            ViewUpdate();
            viewData.dirty = false;
        }

        protected abstract void ViewUpdate();
    }
}