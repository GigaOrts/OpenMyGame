using System;
using System.Threading.Tasks;
using App.Scripts.Infrastructure.GameCore.States.SetupState;
using App.Scripts.Infrastructure.LevelSelection;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels.View.ViewGridLetters;
using App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel;

namespace App.Scripts.Scenes.SceneFillwords.States.Setup
{
    public class HandlerSetupFillwords : IHandlerSetupLevel
    {
        private readonly ContainerGrid _containerGrid;
        private readonly IProviderFillwordLevel _providerFillwordLevel;
        private readonly IServiceLevelSelection _serviceLevelSelection;
        private readonly ViewGridLetters _viewGridLetters;

        public HandlerSetupFillwords(IProviderFillwordLevel providerFillwordLevel,
            IServiceLevelSelection serviceLevelSelection,
            ViewGridLetters viewGridLetters, ContainerGrid containerGrid)
        {
            _providerFillwordLevel = providerFillwordLevel;
            _serviceLevelSelection = serviceLevelSelection;
            _viewGridLetters = viewGridLetters;
            _containerGrid = containerGrid;
        }

        public Task Process()
        {
            GridFillWords model;
            bool canLoadLevel = false;
            int levelIndex = _serviceLevelSelection.CurrentLevelIndex;
            int lastIndex = 0;
            int parseCounter = 0;

            do
            {
                model = _providerFillwordLevel.LoadModel(_serviceLevelSelection.CurrentLevelIndex);

                if (model != null)
                {
                    _viewGridLetters.UpdateItems(model);
                    _containerGrid.SetupGrid(model, _serviceLevelSelection.CurrentLevelIndex);

                    canLoadLevel = true;
                    lastIndex = _serviceLevelSelection.CurrentLevelIndex;
                }
                else
                {
                    levelIndex = _serviceLevelSelection.CurrentLevelIndex;

                    if (levelIndex > lastIndex)
                        levelIndex++;
                    else if (levelIndex < lastIndex)
                        levelIndex--;

                    _serviceLevelSelection.UpdateSelectedLevel(levelIndex);
                }

                parseCounter++;
            }
            while (model == null && levelIndex > 0);

            if (canLoadLevel)
                return Task.CompletedTask;
            else
                throw new Exception();
        }
    }
}