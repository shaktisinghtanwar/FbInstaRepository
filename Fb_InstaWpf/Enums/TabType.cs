using Fb_InstaWpf.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fb_InstaWpf.Enums
{
    public enum TabType
    {
        Messenger,
        Facebook,
        Instagram
    }

    public delegate void SelectedUserChangedDelegate<T>(T socialUser);
    public class PubSubEvent<T>
    {
        static event SelectedUserChangedDelegate<T> SelectedUserChangedEvent;

        List<SelectedUserChangedDelegate<T>> selectedUserChangedDelegates = new List<SelectedUserChangedDelegate<T>>();
        public void Subscribe(SelectedUserChangedDelegate<T> action)
        {
            selectedUserChangedDelegates.Add(action);
        }

        public void Publish(T obj)
        {
            if (selectedUserChangedDelegates.Count > 0)
                foreach (var item in selectedUserChangedDelegates)
                {
                    item(obj);
                }
        }
    }
}
