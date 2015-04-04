/* =======================================================================
 * vCard Library for .NET
 * Copyright (c) 2007-2009 David Pinch; http://wwww.thoughtproject.com
 * See LICENSE.TXT for licensing information.
 * ======================================================================= */

using System;
using System.Collections.Specialized;
using System.IO;

namespace Creek.Tools.VCard
{
    /// <summary>
    ///     A vCard object for exchanging personal contact information.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A vCard contains personal information, such as postal
    ///         addresses, public security certificates, email addresses, and
    ///         web sites.  The vCard specification makes it possible for
    ///         different computer programs to exchange personal contact
    ///         information; for example, a vCard can be attached to an email or
    ///         sent over a wireless connection.
    ///     </para>
    ///     <para>
    ///         The standard vCard format is a text file with properties in
    ///         name:value format.  However, there are multiple versions of
    ///         this format as well as compatible alternatives in XML and
    ///         HTML formats.  This class library aims to accomodate these
    ///         variations but be aware some some formats do not support
    ///         all possible properties.
    ///     </para>
    /// </remarks>
    [Serializable]
    public class vCard
    {
        private readonly StringCollection categories;
        private readonly vCardCertificateCollection certificates;
        private readonly vCardDeliveryAddressCollection deliveryAddresses;
        private readonly vCardDeliveryLabelCollection deliveryLabels;
        private readonly vCardEmailAddressCollection emailAddresses;
        private readonly StringCollection nicknames;
        private readonly vCardNoteCollection notes;
        private readonly vCardPhoneCollection phones;
        private readonly vCardPhotoCollection photos;
        private readonly vCardSourceCollection sources;
        private readonly vCardWebsiteCollection websites;
        private vCardAccessClassification accessClassification;
        private string additionalNames;
        private DateTime? birthDate;
        private string department;
        private string displayName;
        private string familyName;
        private string formattedName;
        private vCardGender gender;
        private string givenName;
        private float? latitude;
        private float? longitude;
        private string mailer;
        private string namePrefix;
        private string nameSuffix;
        private string office;
        private string organization;
        private string productId;
        private DateTime? revisionDate;
        private string role;
        private string timeZone;
        private string title;
        private string uniqueId;

        /// <summary>
        ///     Initializes a new instance of the <see cref="vCard"/> class.
        /// </summary>
        public vCard()
        {
            // Per Microsoft best practices, string properties should
            // never return null.  String properties should always
            // return String.Empty.

            additionalNames = string.Empty;
            department = string.Empty;
            displayName = string.Empty;
            familyName = string.Empty;
            formattedName = string.Empty;
            givenName = string.Empty;
            mailer = string.Empty;
            namePrefix = string.Empty;
            nameSuffix = string.Empty;
            office = string.Empty;
            organization = string.Empty;
            productId = string.Empty;
            role = string.Empty;
            timeZone = string.Empty;
            title = string.Empty;
            uniqueId = string.Empty;

            categories = new StringCollection();
            certificates = new vCardCertificateCollection();
            deliveryAddresses = new vCardDeliveryAddressCollection();
            deliveryLabels = new vCardDeliveryLabelCollection();
            emailAddresses = new vCardEmailAddressCollection();
            nicknames = new StringCollection();
            notes = new vCardNoteCollection();
            phones = new vCardPhoneCollection();
            photos = new vCardPhotoCollection();
            sources = new vCardSourceCollection();
            websites = new vCardWebsiteCollection();
        }


        /// <summary>
        ///     Loads a new instance of the <see cref="vCard"/> class
        ///     from a text reader.
        /// </summary>
        /// <param name="input">
        ///     An initialized text reader.
        /// </param>
        public vCard(TextReader input)
            : this()
        {
            vCardReader reader = new vCardStandardReader();
            reader.ReadInto(this, input);
        }


        /// <summary>
        ///     Loads a new instance of the <see cref="vCard"/> class
        ///     from a text file.
        /// </summary>
        /// <param name="path">
        ///     The path to a text file containing vCard data in
        ///     any recognized vCard format.
        /// </param>
        public vCard(string path)
            : this()
        {
            using (var streamReader = new StreamReader(path))
            {
                vCardReader reader = new vCardStandardReader();
                reader.ReadInto(this, streamReader);
            }
        }


        /// <summary>
        ///     The security access classification of the vCard owner (e.g. private).
        /// </summary>
        public vCardAccessClassification AccessClassification
        {
            get { return accessClassification; }
            set { accessClassification = value; }
        }


        /// <summary>
        ///     Any additional (e.g. middle) names of the person.
        /// </summary>
        /// <seealso cref="FamilyName"/>
        /// <seealso cref="FormattedName"/>
        /// <seealso cref="GivenName"/>
        /// <seealso cref="Nicknames"/>
        public string AdditionalNames
        {
            get { return additionalNames ?? string.Empty; }
            set { additionalNames = value; }
        }


        /// <summary>
        ///     The birthdate of the person.
        /// </summary>
        public DateTime? BirthDate
        {
            get { return birthDate; }
            set { birthDate = value; }
        }


        /// <summary>
        ///     Categories of the vCard.
        /// </summary>
        /// <remarks>
        ///     This property is a collection of strings containing
        ///     keywords or category names.
        /// </remarks>
        public StringCollection Categories
        {
            get { return categories; }
        }


        /// <summary>
        ///     Public key certificates attached to the vCard.
        /// </summary>
        /// <seealso cref="vCardCertificate"/>
        public vCardCertificateCollection Certificates
        {
            get { return certificates; }
        }


        /// <summary>
        ///     Delivery addresses associated with the person.
        /// </summary>
        public vCardDeliveryAddressCollection DeliveryAddresses
        {
            get { return deliveryAddresses; }
        }


        /// <summary>
        ///     Formatted delivery labels.
        /// </summary>
        public vCardDeliveryLabelCollection DeliveryLabels
        {
            get { return deliveryLabels; }
        }


        /// <summary>
        ///     The department of the person in the organization.
        /// </summary>
        /// <seealso cref="Office"/>
        /// <seealso cref="Organization"/>
        public string Department
        {
            get { return department ?? string.Empty; }
            set { department = value; }
        }


        /// <summary>
        ///     The display name of the vCard.
        /// </summary>
        /// <remarks>
        ///     This property is used by vCard applications for titles,
        ///     headers, and other visual elements.
        /// </remarks>
        public string DisplayName
        {
            get { return displayName ?? string.Empty; }
            set { displayName = value; }
        }


        /// <summary>
        ///     A collection of <see cref="vCardEmailAddress"/> objects for the person.
        /// </summary>
        /// <seealso cref="vCardEmailAddress"/>
        public vCardEmailAddressCollection EmailAddresses
        {
            get { return emailAddresses; }
        }


        /// <summary>
        ///     The family (last) name of the person.
        /// </summary>
        /// <seealso cref="AdditionalNames"/>
        /// <seealso cref="FormattedName"/>
        /// <seealso cref="GivenName"/>
        /// <seealso cref="Nicknames"/>
        public string FamilyName
        {
            get { return familyName ?? string.Empty; }
            set { familyName = value; }
        }


        /// <summary>
        ///     The formatted name of the person.
        /// </summary>
        /// <remarks>
        ///     This property allows the name of the person to be
        ///     written in a manner specific to his or her culture.
        ///     The formatted name is not required to strictly
        ///     correspond with the family name, given name, etc.
        /// </remarks>
        /// <seealso cref="AdditionalNames"/>
        /// <seealso cref="FamilyName"/>
        /// <seealso cref="GivenName"/>
        /// <seealso cref="Nicknames"/>
        public string FormattedName
        {
            get { return formattedName ?? string.Empty; }
            set { formattedName = value; }
        }


        /// <summary>
        ///     The gender of the person.
        /// </summary>
        /// <remarks>
        ///     The vCard specification does not define a property
        ///     to indicate the gender of the contact.  Microsoft
        ///     Outlook implements it as a custom property named
        ///     X-WAB-GENDER.
        /// </remarks>
        /// <seealso cref="vCardGender"/>
        public vCardGender Gender
        {
            get { return gender; }
            set { gender = value; }
        }


        /// <summary>
        ///     The given (first) name of the person.
        /// </summary>
        /// <seealso cref="AdditionalNames"/>
        /// <seealso cref="FamilyName"/>
        /// <seealso cref="FormattedName"/>
        /// <seealso cref="Nicknames"/>
        public string GivenName
        {
            get { return givenName ?? string.Empty; }
            set { givenName = value; }
        }


        /// <summary>
        ///     The latitude of the person in decimal degrees.
        /// </summary>
        /// <seealso cref="Longitude"/>
        public float? Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }


        /// <summary>
        ///     The longitude of the person in decimal degrees.
        /// </summary>
        /// <seealso cref="Latitude"/>
        public float? Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }


        /// <summary>
        ///     The mail software used by the person.
        /// </summary>
        public string Mailer
        {
            get { return mailer ?? string.Empty; }
            set { mailer = value; }
        }


        /// <summary>
        ///     The prefix (e.g. "Mr.") of the person.
        /// </summary>
        /// <seealso cref="NameSuffix"/>
        public string NamePrefix
        {
            get { return namePrefix ?? string.Empty; }
            set { namePrefix = value; }
        }


        /// <summary>
        ///     The suffix (e.g. "Jr.") of the person.
        /// </summary>
        /// <seealso cref="NamePrefix"/>
        public string NameSuffix
        {
            get { return nameSuffix ?? string.Empty; }
            set { nameSuffix = value; }
        }


        /// <summary>
        ///     A collection of nicknames for the person.
        /// </summary>
        /// <seealso cref="AdditionalNames"/>
        /// <seealso cref="FamilyName"/>
        /// <seealso cref="FormattedName"/>
        /// <seealso cref="GivenName"/>
        public StringCollection Nicknames
        {
            get { return nicknames; }
        }


        /// <summary>
        ///     A collection of notes or comments.
        /// </summary>
        public vCardNoteCollection Notes
        {
            get { return notes; }
        }


        /// <summary>
        ///     The office of the person at the organization.
        /// </summary>
        /// <seealso cref="Department"/>
        /// <seealso cref="Organization"/>
        public string Office
        {
            get { return office ?? string.Empty; }
            set { office = value; }
        }


        /// <summary>
        ///     The organization or company of the person.
        /// </summary>
        /// <seealso cref="Office"/>
        /// <seealso cref="Role"/>
        /// <seealso cref="Title"/>
        public string Organization
        {
            get { return organization ?? string.Empty; }
            set { organization = value; }
        }


        /// <summary>
        ///     A collection of telephone numbers.
        /// </summary>
        public vCardPhoneCollection Phones
        {
            get { return phones; }
        }


        /// <summary>
        ///     A collection of photographic images embedded or 
        ///     referenced by the vCard.
        /// </summary>
        public vCardPhotoCollection Photos
        {
            get { return photos; }
        }


        /// <summary>
        ///     The name of the product that generated the vCard.
        /// </summary>
        public string ProductId
        {
            get { return productId ?? string.Empty; }
            set { productId = value; }
        }


        /// <summary>
        ///     The revision date of the vCard.
        /// </summary>
        /// <remarks>
        ///     The revision date is not automatically updated by the
        ///     vCard when modifying properties.  It is up to the 
        ///     developer to change the revision date as needed.
        /// </remarks>
        public DateTime? RevisionDate
        {
            get { return revisionDate; }
            set { revisionDate = value; }
        }


        /// <summary>
        ///     The role of the person (e.g. Executive).
        /// </summary>
        /// <remarks>
        ///     The role is shown as "Profession" in Microsoft Outlook.
        /// </remarks>
        /// <seealso cref="Department"/>
        /// <seealso cref="Office"/>
        /// <seealso cref="Organization"/>
        /// <seealso cref="Title"/>
        public string Role
        {
            get { return role ?? string.Empty; }
            set { role = value; }
        }


        /// <summary>
        ///     Directory sources for the vCard information.
        /// </summary>
        /// <remarks>
        ///     A vCard may contain zero or more sources.  A source
        ///     identifies a directory that contains (or provided)
        ///     information found in the vCard.  A program can
        ///     hypothetically connect to the source in order to
        ///     obtain updated information.
        /// </remarks>
        public vCardSourceCollection Sources
        {
            get { return sources; }
        }


        /// <summary>
        ///     A string identifying the time zone of the entity
        ///     represented by the vCard.
        /// </summary>
        public string TimeZone
        {
            get { return timeZone ?? string.Empty; }
            set { timeZone = value; }
        }


        /// <summary>
        ///     The job title of the person.
        /// </summary>
        /// <seealso cref="Organization"/>
        /// <seealso cref="Role"/>
        public string Title
        {
            get { return title ?? string.Empty; }
            set { title = value; }
        }


        /// <summary>
        ///     A value that uniquely identifies the vCard.
        /// </summary>
        /// <remarks>
        ///     This value is optional.  The string must be any string
        ///     that can be used to uniquely identify the vCard.  The
        ///     usage of the field is determined by the software.  Typical
        ///     possibilities for a unique string include a URL, a GUID,
        ///     or an LDAP directory path.  However, there is no particular
        ///     standard dictated by the vCard specification.
        /// </remarks>
        public string UniqueId
        {
            get { return uniqueId ?? string.Empty; }
            set { uniqueId = value; }
        }


        /// <summary>
        ///     Web sites associated with the person.
        /// </summary>
        /// <seealso cref="vCardWebsite"/>
        /// <seealso cref="vCardWebsiteCollection"/>
        public vCardWebsiteCollection Websites
        {
            get { return websites; }
        }

        /// <summary>
        ///     Builds a string that represents the vCard.
        /// </summary>
        /// <returns>
        ///     The formatted name of the contact person, if defined,
        ///     or the default object.ToString().
        /// </returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(formattedName))
            {
                return base.ToString();
            }
            else
            {
                return formattedName;
            }
        }
    }
}