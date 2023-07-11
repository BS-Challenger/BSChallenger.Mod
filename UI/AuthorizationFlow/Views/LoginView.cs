using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using SiraUtil.Affinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSChallengeRanking.UI.AuthorizationFlow.Views
{
	[HotReload(RelativePathToLayout = @"LoginView.bsml")]
	[ViewDefinition("BSChallengeRanking.UI.AuthorizationFlow.Views.LoginView.bsml")]
	internal class LoginView : BSMLAutomaticViewController
	{
	}
}
