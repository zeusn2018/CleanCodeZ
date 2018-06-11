using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer;
using System.Collections.Generic;

namespace BusinessLayer.Tests
{
	[TestClass]
	public class SpeakerTests
	{
        private SqlServerCompactRepository repository = new SqlServerCompactRepository(); 

		[TestMethod]
		public void Register_EmptyFirstName_ThrowsArgumentNullException()
		{			
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.FirstName = "";			
		
			var exception = ExceptionAssert.Throws<ArgumentNullException>( () => speaker.Register(repository));			
			Assert.AreEqual(exception.GetType(), typeof(ArgumentNullException));
		}

		[TestMethod]
		public void Register_EmptyLastName_ThrowsArgumentNullException()
		{			
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.LastName = "";
			
			var exception = ExceptionAssert.Throws<ArgumentNullException>(() => speaker.Register(repository));			
			Assert.AreEqual(exception.GetType(), typeof(ArgumentNullException));
		}

		[TestMethod]
		public void Register_EmptyEmail_ThrowsArgumentNullException()
		{		
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.Email = "";
			
			var exception = ExceptionAssert.Throws<ArgumentNullException>(() => speaker.Register(repository));			
			Assert.AreEqual(exception.GetType(), typeof(ArgumentNullException));
		}

		[TestMethod]
		public void Register_WorksForPrestigiousEmployerButHasRedFlags_ReturnsSpeakerId()
		{			
			var speaker = GetSpeakerWithRedFlags();
			speaker.Employer = "Microsoft";
			
			int? speakerId = speaker.Register(new SqlServerCompactRepository());			
			Assert.IsFalse(speakerId == null);
		}

		[TestMethod]
		public void Register_HasBlogButHasRedFlags_ReturnsSpeakerId()
		{			
			var speaker = GetSpeakerWithRedFlags();			
			int? speakerId = speaker.Register(new SqlServerCompactRepository());			
			Assert.IsFalse(speakerId == null);
		}

		[TestMethod]
		public void Register_HasCertificationsButHasRedFlags_ReturnsSpeakerId()
		{			
			var speaker = GetSpeakerWithRedFlags();
			speaker.Certifications = new List<string>()
			{
				"cert1",
				"cert2",
				"cert3",
				"cert4"
			};
			
			int? speakerId = speaker.Register(new SqlServerCompactRepository());			
			Assert.IsFalse(speakerId == null);
		}

		[TestMethod]
		public void Register_SingleSessionThatsOnOldTech_ThrowsNoSessionsApprovedException()
		{			
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.Sessions = new List<Session>() {
				new Session("Cobol for dummies", "Intro to Cobol")
			};
			
			var exception = ExceptionAssert.Throws<BusinessLayer.Speaker.NoSessionsApprovedException>(() => speaker.Register(repository));
			Assert.AreEqual(exception.GetType(), typeof(Speaker.NoSessionsApprovedException));
		}

		[TestMethod]
		public void Register_NoSessionsPassed_ThrowsArgumentException()
		{			
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.Sessions = new List<Session>();
			
			var exception = ExceptionAssert.Throws<ArgumentException>(() => speaker.Register(repository));			
			Assert.AreEqual(exception.GetType(), typeof(ArgumentException));
		}

		[TestMethod]
		public void Register_DoesntAppearExceptionalAndUsingOldBrowser_ThrowsNoSessionsApprovedException()
		{
			var speakerThatDoesntAppearExceptional = GetSpeakerThatWouldBeApproved();
			speakerThatDoesntAppearExceptional.HasBlog = false;
			speakerThatDoesntAppearExceptional.Browser = new WebBrowser("IE", 6);
			
			var exception = ExceptionAssert.Throws<BusinessLayer.Speaker.SpeakerDoesntMeetRequirementsException>(() => speakerThatDoesntAppearExceptional.Register(repository));			
			Assert.AreEqual(exception.GetType(), typeof(Speaker.SpeakerDoesntMeetRequirementsException));
		}

		[TestMethod]
		public void Register_DoesntAppearExceptionalAndHasAncientEmail_ThrowsNoSessionsApprovedException()
		{			
			var speakerThatDoesntAppearExceptional = GetSpeakerThatWouldBeApproved();
			speakerThatDoesntAppearExceptional.HasBlog = false;
			speakerThatDoesntAppearExceptional.Email = "name@aol.com";
			
			var exception = ExceptionAssert.Throws<BusinessLayer.Speaker.SpeakerDoesntMeetRequirementsException>(() => speakerThatDoesntAppearExceptional.Register(repository));                        			
			Assert.AreEqual(exception.GetType(), typeof(Speaker.SpeakerDoesntMeetRequirementsException));
		}

		#region Helpers
		private Speaker GetSpeakerThatWouldBeApproved()
		{
			return new Speaker()
			{
				FirstName = "First",
				LastName = "Last",
				Email = "example@domain.com",
				Employer = "Example Employer",
				HasBlog = true,
				Browser = new WebBrowser("test", 1),
				YearsExperience = 1,
				Certifications = new System.Collections.Generic.List<string>(),
				BlogURL = "",
				Sessions = new System.Collections.Generic.List<Session>() {
					new Session("test title", "test description")
				}
			};
		}

		private Speaker GetSpeakerWithRedFlags()
		{
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.Email = "tom@aol.com";
			speaker.Browser = new WebBrowser("IE", 6);
			return speaker;
		}
		#endregion
	}
}
