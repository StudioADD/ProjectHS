using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MomDra
{
    public class TitleScenePresenter : PresenterBase
    {
        public TitleScenePresenter(ViewBase view) : base(view)
        {
            // Model의 이벤트 등록
        }

        public void ChangeView(ViewBase view)
        {
            this.view = view;
        }

        public void PlayButtonClicked()
        {
            
        }
    }
}
