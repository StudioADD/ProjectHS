using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MomDra
{
    public class PresenterBase
    {
        protected ViewBase view;

        public PresenterBase(ViewBase view)
        {
            this.view = view;
            view.SetPresenter(this);
        }
    }
}
