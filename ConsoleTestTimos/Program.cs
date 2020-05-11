using sc2i.common;
using sc2i.data;
using sc2i.multitiers.client;
using sc2i.process;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using tiag.client;
using timos.client;
using timos.data.Aspectize;
using timos.securite;

namespace ConsoleTestTimos
{
    class Program
    {
        static void Main(string[] args)
        {
            CResultAErreur result = CResultAErreur.True;
            string strServeurUrl = "tcp://127.0.0.1:8160";
            int nTcpChannel = 0;
            string strBindTo = "";

            try
            {
                AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

                //SUPPRIMé journal d'évenements: sur les postes clients qui ne sont pas autorisés
                //à créer un journal d'évenements, ça bloque, et comme ce n'est pas
                //très important sur un poste client, il n'y a plus 
                //de journal d'évenements TIMOS sur les postes clients.
                //C2iEventLog.Init("", "Client Timos", NiveauBavardage.VraiPiplette);

                result = CSC2iMultitiersClient.Init(nTcpChannel, strServeurUrl, strBindTo);

                LifetimeServices.LeaseTime = new TimeSpan(0, 5, 0);
                LifetimeServices.LeaseManagerPollTime = new TimeSpan(0, 5, 0);
                LifetimeServices.SponsorshipTimeout = new TimeSpan(0, 3, 0);
                LifetimeServices.RenewOnCallTime = new TimeSpan(0, 8, 0);

                C2iSponsor.EnableSecurite();

                if (result)
                {
                    /*CSessionClient session = CSessionClient.CreateInstance();

                    CAuthentificationSessionTimosLoginPwd authParams = new CAuthentificationSessionTimosLoginPwd(
                        "youcef",
                        "minutes",
                        new CParametresLicence(new List<string>(), new List<string>()));

                    //result = session.OpenSession(authParams, "Console de test", ETypeApplicationCliente.Windows);/
                    result = session.OpenSession(new CAuthentificationSessionProcess(), "Console de test", ETypeApplicationCliente.Process);

                    if (!result)
                    {
                        result.EmpileErreur("Erreur lors de l'authentification");
                        Console.WriteLine("Erreur lors de l'authentification");
                        Console.ReadKey();
                        return;
                    }

                    /*
                    string strLogin = "youcef";
                    string strPassCrypte = C2iCrypto.Crypte("minutes");

                    CContexteDonnee contexte = new CContexteDonnee(session.IdSession, true, false);
                    CDonneesActeurUtilisateur utilisateurTimos = new CDonneesActeurUtilisateur(contexte);
                    if (utilisateurTimos.ReadIfExists(new CFiltreData(CDonneesActeurUtilisateur.c_champLogin + "=@1 and " +
                            CDonneesActeurUtilisateur.c_champPassword + "=@2",
                            strLogin, strPassCrypte)))
                    {
                        result.Data = utilisateurTimos;
                        Console.WriteLine("Utilisateur " + strLogin + " connecté");
                        Console.ReadKey();
                        return;
                    }*/

                    ITimosServiceForAspectize serviceClientAspectize = (ITimosServiceForAspectize)C2iFactory.GetNewObject(typeof(ITimosServiceForAspectize));
                    string strNomGestionnaire = serviceClientAspectize.GetType().ToString();
                    result = serviceClientAspectize.OpenSession("youcef", "minutes");
                    if(result)
                    {
                        Console.WriteLine("Key utilisateur connecté = " + (string)result.Data);
                        Console.ReadKey();
                    }

                    Console.WriteLine("Nom du gestionnaire : " + strNomGestionnaire);
                    Console.ReadKey();

                }
            }
            catch (Exception e)
            {
                result.EmpileErreur(e.Message);
                Console.WriteLine(e.Message);
                Console.WriteLine("Erreur. Pressez n'importe quelle touche pour quitter");
                Console.ReadKey();
            }

        }
    }
}
