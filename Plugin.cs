using BSChallenger.Installers;
using IPA;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;

namespace BSChallenger
{
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin
	{
		internal static Plugin Instance { get; private set; }

		[Init]
		public Plugin(IPALogger logger, Zenjector zenjector)
		{
			Instance = this;
			zenjector.Install<BSChallengerMenuInstaller>(Location.Menu);
			zenjector.UseSiraSync(SiraUtil.Web.SiraSync.SiraSyncServiceType.GitHub, "Saber-Quest", "SaberQuest-Mod");
			zenjector.UseLogger(logger);
			zenjector.UseHttpService();
			zenjector.UseMetadataBinder<Plugin>();
		}
	}
}
