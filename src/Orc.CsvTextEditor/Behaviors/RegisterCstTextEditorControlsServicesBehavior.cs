﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterCstTextEditorControlsServicesBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CsvTextEditor
{
    using System.ComponentModel;
    using Catel.IoC;
    using Catel.Windows.Interactivity;
    using Services;

    internal class RegisterCstTextEditorControlsServicesBehavior : BehaviorBase<CsvTextEditorControl>
    {
        #region Fields
        private readonly IServiceLocator _serviceLocator;
        private readonly ITypeFactory _typeFactory;
        #endregion

        #region Constructors
        public RegisterCstTextEditorControlsServicesBehavior()
        {
            _serviceLocator = this.GetServiceLocator();
            _typeFactory = _serviceLocator.ResolveType<ITypeFactory>();
        }
        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();

            UpdateServiceRegistration();

            var textEditorControl = AssociatedObject;
            textEditorControl.PropertyChanged += OnTextEditorControlPropertyChanged;
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            var textEditorControl = AssociatedObject;
            textEditorControl.PropertyChanged -= OnTextEditorControlPropertyChanged;

            base.OnAssociatedObjectUnloaded();
        }

        private void OnTextEditorControlPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
#pragma warning disable WPF1014
            if (args.HasPropertyChanged(nameof(AssociatedObject.Scope)))
#pragma warning restore WPF1014
            {
                UpdateServiceRegistration();
            }
        }

        private void UpdateServiceRegistration()
        {
            var textEditorControl = AssociatedObject;
            var textEditor = textEditorControl.TextEditor;
            if (textEditor == null)
            {
                return;
            }

            var scope = textEditorControl.Scope;
            if (!_serviceLocator.IsTypeRegistered<ICsvTextEditorService>(scope))
            {
                var csvTextEditorService = (ICsvTextEditorService) _typeFactory.CreateInstanceWithParametersAndAutoCompletion<CsvTextEditorService>(textEditor);
                _serviceLocator.RegisterInstance(csvTextEditorService, scope);
            }

            if (!_serviceLocator.IsTypeRegistered<ICsvTextSynchronizationService>(scope))
            {
                var csvTextSynchronizationService = (ICsvTextSynchronizationService) _typeFactory.CreateInstanceWithParametersAndAutoCompletion<CsvTextSynchronizationService>();
                _serviceLocator.RegisterInstance(csvTextSynchronizationService, scope);
            }
        }
    }
}