﻿using System.Linq;

using ClassicUO.Game.GameObjects;
using ClassicUO.Game.Scenes;

namespace ClassicUO.Game.Gumps.UIGumps.CharCreation
{
    internal class CharCreationGump : Gump
    {
        public enum CharCreationStep
        {
            Appearence = 0,
            ChooseTrade = 1
        }

        private PlayerMobile _character;
        private CharCreationStep _currentStep;
        private LoadingGump _loadingGump;
        private readonly LoginScene loginScene;

        public CharCreationGump() : base(0, 0)
        {
            loginScene = Service.Get<LoginScene>();
            AddChildren(new CreateCharAppearanceGump(), 1);
            SetStep(CharCreationStep.Appearence);
            CanCloseWithRightClick = false;
            Service.Register(this);
        }

        public void SetCharacter(PlayerMobile character)
        {
            _character = character;
            SetStep(CharCreationStep.ChooseTrade);
        }

        public void CreateCharacter()
        {
            loginScene.CreateCharacter(_character);
            Service.Unregister<CharCreationGump>();
        }

        public void StepBack()
        {
            if (_currentStep == CharCreationStep.Appearence)
            {
                Service.Unregister<CharCreationGump>();
                loginScene.StepBack();
            }
            else
                SetStep(_currentStep - 1);
        }

        public void ShowMessage(string message)
        {
            var currentPage = ActivePage;

            if (_loadingGump != null)
                RemoveChildren(_loadingGump);
            AddChildren(_loadingGump = new LoadingGump(message, LoadingGump.Buttons.OK, a => ChangePage(currentPage)), 4);
            ChangePage(4);
        }

        private void SetStep(CharCreationStep step)
        {
            _currentStep = step;

            switch (step)
            {
                case CharCreationStep.Appearence:
                    ChangePage(1);

                    break;
                case CharCreationStep.ChooseTrade:
                    var existent = Children.Where(o => o.Page == 2).FirstOrDefault();

                    if (existent != null)
                        RemoveChildren(existent);
                    AddChildren(new CreateCharTradeGump(_character), 2);
                    ChangePage(2);

                    break;
            }
        }
    }
}