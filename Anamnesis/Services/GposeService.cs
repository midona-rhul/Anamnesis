﻿// Concept Matrix 3.
// Licensed under the MIT license.

namespace Anamnesis.Services
{
	using System.Threading.Tasks;
	using Anamnesis.Core.Memory;
	using Anamnesis.Memory;
	using PropertyChanged;

	[AddINotifyPropertyChangedInterface]
	public class GposeService : ServiceBase<GposeService>
	{
		public bool IsGpose { get; private set; }
		public bool IsOverworld { get; private set; }

		public bool IsChangingState { get; private set; }

		public override Task Start()
		{
			Task.Run(this.CheckThread);
			return base.Start();
		}

		private async Task CheckThread()
		{
			while (this.IsAlive)
			{
				byte check1 = MemoryService.Read<byte>(AddressService.GposeCheck);
				byte check2 = MemoryService.Read<byte>(AddressService.GposeCheck2);
				bool newGpose = check1 == 1 && check2 == 4;

				if (newGpose != this.IsGpose)
				{
					this.IsChangingState = true;
					TargetService.Instance.ClearSelection();
					await Task.Delay(500);
				}

				this.IsGpose = newGpose;
				this.IsOverworld = !this.IsGpose;
				this.IsChangingState = false;

				await Task.Delay(100);
			}
		}
	}
}
