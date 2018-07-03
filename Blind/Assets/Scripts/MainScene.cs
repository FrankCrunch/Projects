using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour {

	[SerializeField]
	GameObject[] pointOnTable;
	[SerializeField]
	Text screen;
	
	GameObject[] pointOnCabinet;
	string[] tags;
	int time = 600;
	
	void Start () {
		pointOnCabinet = new GameObject[]{null, null, null, null, null};
		tags = new string[] {"", "", "", "", ""};
		//StartCoroutine(WaidAndChangeTimer());
	}
	
	public void PointerEnter(GameObject obj){
		StartCoroutine("WaitAndReplace", obj);
	}
	
	public void PointerExit(){
		StopCoroutine("WaitAndReplace");
	}
	
	IEnumerator WaitAndReplace(GameObject obj){
		yield return new WaitForSeconds(1f);
		ReplaceObject(obj);
	}
	
	public void ReplaceObject(GameObject obj){
		for (int i = 0; i < 5; i++){
			if (pointOnTable[i].transform.position == obj.transform.position) {
				obj.transform.position = pointOnCabinet[i].transform.position;
				pointOnCabinet[i] = null;
				tags[i] = "";
				return;
			}
		}
		for (int i = 0; i < 5; i++){
			if (pointOnCabinet[i] == null){
				pointOnCabinet[i] = new GameObject(); 
				pointOnCabinet[i].transform.position = obj.transform.position;
				tags[i] = obj.tag;
				obj.transform.position = pointOnTable[i].transform.position;
				break;
			}
		}
		
	}
	
	int NumberOfObjects(){
		int num = 0;
		for (int i = 0; i < 5; i++)
			if (pointOnCabinet[i] != null) num++;
		return num;
	}
	
	IEnumerator WaidAndChangeTimer(){
		while (time > 0) {
			yield return new WaitForSeconds(0.1f);
			time--;
			
			screen.text = (time/10.0).ToString("N") + "\n" + NumberOfObjects().ToString() + " / 5";
		}
		StartCoroutine(WaitAndEndScene());
	}
	
	IEnumerator WaitAndEndScene(){
		yield return new WaitForSeconds(1f);
		EndScene();
	}
	
	public void EndScenePointerEnter(){
		StartCoroutine("WaitAndEndScene");
	}
	
	public void EndScenePointerExit(){
		StopCoroutine("WaitAndEndScene");
	}
	
	public void EndScene(){
		if (PlayerPrefs.GetString("Email") != "") {
			int swords = 0, lifePots = 0, manaPots = 0, chests = 0, arrows = 0, staffs = 0;
		
			for (int i = 0; i < 5; i++) {
				if (tags[i] == "Sword") swords++;
				if (tags[i] == "LifePot") lifePots++;
				if (tags[i] == "ManaPot") manaPots++;
				if (tags[i] == "Arrow") arrows++;
				if (tags[i] == "Staff") staffs++;
				if (tags[i] == "Chest") chests++;
			}
		
			string mailText = "";
			if (PlayerPrefs.GetString("FullName") != "")
				mailText += "Dear " + PlayerPrefs.GetString("FullName") + "!\n";
			
			mailText += "Your results:\n";
		
			if (swords == 2) mailText += "I am Spartacus! (dual swords): 100$\n";
			else if (swords > 0) mailText += "Swords: " + swords + " * 30$ = " + (swords * 30) + "$\n";
		
			if (lifePots == 4) mailText += "Dont want to die! (4 life pots): 100$\n";
			else if (lifePots > 0) mailText += "Life pots: " + lifePots + " * 5$ = " + (lifePots * 5) + "$\n";
		
			if (manaPots > 0) mailText += "Mana pots: " + manaPots + " * 3$ = " + (manaPots * 3) + "$\n";
		
			if (chests == 4) mailText += "I have nothing to wear! (4 сhests): 100$\n";
			else if (chests > 0) mailText += "Chests: " + chests + " * 10$ = " + (chests * 10) + "$\n";
		
			if (arrows > 0) mailText += "Arrows: " + arrows + " * 1$ = " + (arrows * 1) + "$\n";
			if (staffs > 0) mailText += "Staffs: " + staffs + " * 1$ = " + (staffs * 50) + "$\n";
			
			MailMessage mail = new MailMessage();
 
			mail.From = new MailAddress("sketchesgames@gmail.com");
			mail.To.Add(PlayerPrefs.GetString("Email"));
			mail.Subject = "Properties";
			mail.Body = mailText;
 
			//SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
			//smtpServer.Port = 587;
			SmtpClient smtpServer = new SmtpClient();
			smtpServer.Host = "smtp.gmail.com";
			smtpServer.Port = 587;
			smtpServer.Credentials = new System.Net.NetworkCredential("sketchesgames@gmail.com", "sdf343@kdn") as ICredentialsByHost;
			smtpServer.EnableSsl = true;
			ServicePointManager.ServerCertificateValidationCallback = 
				delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) 
				{ return true; };
			smtpServer.Send(mail);
		}
		
		SceneManager.LoadScene("LastScene");
	}
	
	void Update () {
		
	}
}
