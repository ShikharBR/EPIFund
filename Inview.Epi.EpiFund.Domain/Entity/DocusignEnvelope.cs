using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class DocusignEnvelope
	{
		public string AssetNumbers
		{
			get;
			set;
		}

		public DateTime DateCreated
		{
			get;
			set;
		}

		public DateTime? DateReceived
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.DocumentType DocumentType
		{
			get;
			set;
		}

		public int DocusignEnvelopeId
		{
			get;
			set;
		}

		public string EnvelopeId
		{
			get;
			set;
		}

		public bool ReceivedSignedDocument
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public DocusignEnvelope()
		{
		}
	}
}