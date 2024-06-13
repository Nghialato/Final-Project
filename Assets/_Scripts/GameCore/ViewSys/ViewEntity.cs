using UnityEngine;

namespace _Scripts.GameCore.ViewSys
{
    public class ViewEntity : MonoBehaviour, IView
    {
        public ViewData viewData;
        public void RegisterToSystem()
        {
            ViewSystemManagerEts.RegisterToArray(this);
        }

        public void RemoveFromSystem()
        {
            ViewSystemManagerEts.RemoveFromArray(this);
            this.gameObject.SetActive(false);
        }

        public void UpdateComponent()
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