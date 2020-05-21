﻿// Concept Matrix 3.
// Licensed under the MIT license.

namespace ConceptMatrix.PoseModule
{
	using System.Threading.Tasks;
	using System.Windows;
	using ConceptMatrix.Modules;
	using ConceptMatrix.PoseModule.Pages;

	public class Module : IModule
	{
		public static SkeletonViewModel SkeletonViewModel { get; set; } = new SkeletonViewModel();

		public Task Initialize()
		{
			Services.Add<SkeletonService>();

			IViewService viewService = Services.Get<IViewService>();
			viewService.AddPage<Pose3DPage>("Character/Pose 3D", false);
			viewService.AddPage<PoseGuiPage>("Character/Posing GUI", false);
			viewService.AddPage<PoseMatrixPage>("Character/Posing Matrix", false);
			viewService.AddPage<PositionPage>("Character/Positioning");

			ISelectionService selectionService = Services.Get<ISelectionService>();
			selectionService.SelectionChanged += this.OnSelectionChanged;
			this.OnSelectionChanged(selectionService.CurrentSelection);

			Application.Current.Dispatcher.Invoke(() =>
			{
				Application.Current.Exit += this.OnApplicationExiting;
			});

			return Task.CompletedTask;
		}

		public Task Start()
		{
			return Task.CompletedTask;
		}

		public Task Shutdown()
		{
			return Task.CompletedTask;
		}

		private void OnSelectionChanged(Selection selection)
		{
			SkeletonViewModel.Clear();

			if (selection == null)
				return;

			Application.Current.Dispatcher.Invoke(async () =>
			{
				await SkeletonViewModel.Initialize(selection);
			});
		}

		private void OnApplicationExiting(object sender, ExitEventArgs e)
		{
			if (SkeletonViewModel == null)
				return;

			SkeletonViewModel.IsEnabled = false;
		}
	}
}