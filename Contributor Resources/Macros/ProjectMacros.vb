'*******************************************************************************************************
'  ProjectMacros.vb - Gbtc
'
'  Tennessee Valley Authority, 2009
'  No copyright is claimed pursuant to 17 USC § 105.  All Other Rights Reserved.
'
'  This software is made freely available under the TVA Open Source Agreement (see below).
'
'  Code Modification History:
'  -----------------------------------------------------------------------------------------------------
'  02/08/2009 - James R. Carroll
'       Generated original version of source code.
'
'*******************************************************************************************************

#Region " TVA Open Source Agreement "

' THIS OPEN SOURCE AGREEMENT ("AGREEMENT") DEFINES THE RIGHTS OF USE,REPRODUCTION, DISTRIBUTION,
' MODIFICATION AND REDISTRIBUTION OF CERTAIN COMPUTER SOFTWARE ORIGINALLY RELEASED BY THE
' TENNESSEE VALLEY AUTHORITY, A CORPORATE AGENCY AND INSTRUMENTALITY OF THE UNITED STATES GOVERNMENT
' ("GOVERNMENT AGENCY"). GOVERNMENT AGENCY IS AN INTENDED THIRD-PARTY BENEFICIARY OF ALL SUBSEQUENT
' DISTRIBUTIONS OR REDISTRIBUTIONS OF THE SUBJECT SOFTWARE. ANYONE WHO USES, REPRODUCES, DISTRIBUTES,
' MODIFIES OR REDISTRIBUTES THE SUBJECT SOFTWARE, AS DEFINED HEREIN, OR ANY PART THEREOF, IS, BY THAT
' ACTION, ACCEPTING IN FULL THE RESPONSIBILITIES AND OBLIGATIONS CONTAINED IN THIS AGREEMENT.

' Original Software Designation: openPDC
' Original Software Title: The TVA Open Source Phasor Data Concentrator
' User Registration Requested. Please Visit https://naspi.tva.com/Registration/
' Point of Contact for Original Software: J. Ritchie Carroll <jrcarrol@tva.gov>

' 1. DEFINITIONS

' A. "Contributor" means Government Agency, as the developer of the Original Software, and any entity
' that makes a Modification.

' B. "Covered Patents" mean patent claims licensable by a Contributor that are necessarily infringed by
' the use or sale of its Modification alone or when combined with the Subject Software.

' C. "Display" means the showing of a copy of the Subject Software, either directly or by means of an
' image, or any other device.

' D. "Distribution" means conveyance or transfer of the Subject Software, regardless of means, to
' another.

' E. "Larger Work" means computer software that combines Subject Software, or portions thereof, with
' software separate from the Subject Software that is not governed by the terms of this Agreement.

' F. "Modification" means any alteration of, including addition to or deletion from, the substance or
' structure of either the Original Software or Subject Software, and includes derivative works, as that
' term is defined in the Copyright Statute, 17 USC § 101. However, the act of including Subject Software
' as part of a Larger Work does not in and of itself constitute a Modification.

' G. "Original Software" means the computer software first released under this Agreement by Government
' Agency entitled openPDC, including source code, object code and accompanying documentation, if any.

' H. "Recipient" means anyone who acquires the Subject Software under this Agreement, including all
' Contributors.

' I. "Redistribution" means Distribution of the Subject Software after a Modification has been made.

' J. "Reproduction" means the making of a counterpart, image or copy of the Subject Software.

' K. "Sale" means the exchange of the Subject Software for money or equivalent value.

' L. "Subject Software" means the Original Software, Modifications, or any respective parts thereof.

' M. "Use" means the application or employment of the Subject Software for any purpose.

' 2. GRANT OF RIGHTS

' A. Under Non-Patent Rights: Subject to the terms and conditions of this Agreement, each Contributor,
' with respect to its own contribution to the Subject Software, hereby grants to each Recipient a
' non-exclusive, world-wide, royalty-free license to engage in the following activities pertaining to
' the Subject Software:

' 1. Use

' 2. Distribution

' 3. Reproduction

' 4. Modification

' 5. Redistribution

' 6. Display

' B. Under Patent Rights: Subject to the terms and conditions of this Agreement, each Contributor, with
' respect to its own contribution to the Subject Software, hereby grants to each Recipient under Covered
' Patents a non-exclusive, world-wide, royalty-free license to engage in the following activities
' pertaining to the Subject Software:

' 1. Use

' 2. Distribution

' 3. Reproduction

' 4. Sale

' 5. Offer for Sale

' C. The rights granted under Paragraph B. also apply to the combination of a Contributor's Modification
' and the Subject Software if, at the time the Modification is added by the Contributor, the addition of
' such Modification causes the combination to be covered by the Covered Patents. It does not apply to
' any other combinations that include a Modification. 

' D. The rights granted in Paragraphs A. and B. allow the Recipient to sublicense those same rights.
' Such sublicense must be under the same terms and conditions of this Agreement.

' 3. OBLIGATIONS OF RECIPIENT

' A. Distribution or Redistribution of the Subject Software must be made under this Agreement except for
' additions covered under paragraph 3H. 

' 1. Whenever a Recipient distributes or redistributes the Subject Software, a copy of this Agreement
' must be included with each copy of the Subject Software; and

' 2. If Recipient distributes or redistributes the Subject Software in any form other than source code,
' Recipient must also make the source code freely available, and must provide with each copy of the
' Subject Software information on how to obtain the source code in a reasonable manner on or through a
' medium customarily used for software exchange.

' B. Each Recipient must ensure that the following copyright notice appears prominently in the Subject
' Software:

'          No copyright is claimed pursuant to 17 USC § 105.  All Other Rights Reserved.

' C. Each Contributor must characterize its alteration of the Subject Software as a Modification and
' must identify itself as the originator of its Modification in a manner that reasonably allows
' subsequent Recipients to identify the originator of the Modification. In fulfillment of these
' requirements, Contributor must include a file (e.g., a change log file) that describes the alterations
' made and the date of the alterations, identifies Contributor as originator of the alterations, and
' consents to characterization of the alterations as a Modification, for example, by including a
' statement that the Modification is derived, directly or indirectly, from Original Software provided by
' Government Agency. Once consent is granted, it may not thereafter be revoked.

' D. A Contributor may add its own copyright notice to the Subject Software. Once a copyright notice has
' been added to the Subject Software, a Recipient may not remove it without the express permission of
' the Contributor who added the notice.

' E. A Recipient may not make any representation in the Subject Software or in any promotional,
' advertising or other material that may be construed as an endorsement by Government Agency or by any
' prior Recipient of any product or service provided by Recipient, or that may seek to obtain commercial
' advantage by the fact of Government Agency's or a prior Recipient's participation in this Agreement.

' F. In an effort to track usage and maintain accurate records of the Subject Software, each Recipient,
' upon receipt of the Subject Software, is requested to register with Government Agency by visiting the
' following website: https://naspi.tva.com/Registration/. Recipient's name and personal information
' shall be used for statistical purposes only. Once a Recipient makes a Modification available, it is
' requested that the Recipient inform Government Agency at the web site provided above how to access the
' Modification.

' G. Each Contributor represents that that its Modification does not violate any existing agreements,
' regulations, statutes or rules, and further that Contributor has sufficient rights to grant the rights
' conveyed by this Agreement.

' H. A Recipient may choose to offer, and to charge a fee for, warranty, support, indemnity and/or
' liability obligations to one or more other Recipients of the Subject Software. A Recipient may do so,
' however, only on its own behalf and not on behalf of Government Agency or any other Recipient. Such a
' Recipient must make it absolutely clear that any such warranty, support, indemnity and/or liability
' obligation is offered by that Recipient alone. Further, such Recipient agrees to indemnify Government
' Agency and every other Recipient for any liability incurred by them as a result of warranty, support,
' indemnity and/or liability offered by such Recipient.

' I. A Recipient may create a Larger Work by combining Subject Software with separate software not
' governed by the terms of this agreement and distribute the Larger Work as a single product. In such
' case, the Recipient must make sure Subject Software, or portions thereof, included in the Larger Work
' is subject to this Agreement.

' J. Notwithstanding any provisions contained herein, Recipient is hereby put on notice that export of
' any goods or technical data from the United States may require some form of export license from the
' U.S. Government. Failure to obtain necessary export licenses may result in criminal liability under
' U.S. laws. Government Agency neither represents that a license shall not be required nor that, if
' required, it shall be issued. Nothing granted herein provides any such export license.

' 4. DISCLAIMER OF WARRANTIES AND LIABILITIES; WAIVER AND INDEMNIFICATION

' A. No Warranty: THE SUBJECT SOFTWARE IS PROVIDED "AS IS" WITHOUT ANY WARRANTY OF ANY KIND, EITHER
' EXPRESSED, IMPLIED, OR STATUTORY, INCLUDING, BUT NOT LIMITED TO, ANY WARRANTY THAT THE SUBJECT
' SOFTWARE WILL CONFORM TO SPECIFICATIONS, ANY IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
' PARTICULAR PURPOSE, OR FREEDOM FROM INFRINGEMENT, ANY WARRANTY THAT THE SUBJECT SOFTWARE WILL BE ERROR
' FREE, OR ANY WARRANTY THAT DOCUMENTATION, IF PROVIDED, WILL CONFORM TO THE SUBJECT SOFTWARE. THIS
' AGREEMENT DOES NOT, IN ANY MANNER, CONSTITUTE AN ENDORSEMENT BY GOVERNMENT AGENCY OR ANY PRIOR
' RECIPIENT OF ANY RESULTS, RESULTING DESIGNS, HARDWARE, SOFTWARE PRODUCTS OR ANY OTHER APPLICATIONS
' RESULTING FROM USE OF THE SUBJECT SOFTWARE. FURTHER, GOVERNMENT AGENCY DISCLAIMS ALL WARRANTIES AND
' LIABILITIES REGARDING THIRD-PARTY SOFTWARE, IF PRESENT IN THE ORIGINAL SOFTWARE, AND DISTRIBUTES IT
' "AS IS."

' B. Waiver and Indemnity: RECIPIENT AGREES TO WAIVE ANY AND ALL CLAIMS AGAINST GOVERNMENT AGENCY, ITS
' AGENTS, EMPLOYEES, CONTRACTORS AND SUBCONTRACTORS, AS WELL AS ANY PRIOR RECIPIENT. IF RECIPIENT'S USE
' OF THE SUBJECT SOFTWARE RESULTS IN ANY LIABILITIES, DEMANDS, DAMAGES, EXPENSES OR LOSSES ARISING FROM
' SUCH USE, INCLUDING ANY DAMAGES FROM PRODUCTS BASED ON, OR RESULTING FROM, RECIPIENT'S USE OF THE
' SUBJECT SOFTWARE, RECIPIENT SHALL INDEMNIFY AND HOLD HARMLESS  GOVERNMENT AGENCY, ITS AGENTS,
' EMPLOYEES, CONTRACTORS AND SUBCONTRACTORS, AS WELL AS ANY PRIOR RECIPIENT, TO THE EXTENT PERMITTED BY
' LAW.  THE FOREGOING RELEASE AND INDEMNIFICATION SHALL APPLY EVEN IF THE LIABILITIES, DEMANDS, DAMAGES,
' EXPENSES OR LOSSES ARE CAUSED, OCCASIONED, OR CONTRIBUTED TO BY THE NEGLIGENCE, SOLE OR CONCURRENT, OF
' GOVERNMENT AGENCY OR ANY PRIOR RECIPIENT.  RECIPIENT'S SOLE REMEDY FOR ANY SUCH MATTER SHALL BE THE
' IMMEDIATE, UNILATERAL TERMINATION OF THIS AGREEMENT.

' 5. GENERAL TERMS

' A. Termination: This Agreement and the rights granted hereunder will terminate automatically if a
' Recipient fails to comply with these terms and conditions, and fails to cure such noncompliance within
' thirty (30) days of becoming aware of such noncompliance. Upon termination, a Recipient agrees to
' immediately cease use and distribution of the Subject Software. All sublicenses to the Subject
' Software properly granted by the breaching Recipient shall survive any such termination of this
' Agreement.

' B. Severability: If any provision of this Agreement is invalid or unenforceable under applicable law,
' it shall not affect the validity or enforceability of the remainder of the terms of this Agreement.

' C. Applicable Law: This Agreement shall be subject to United States federal law only for all purposes,
' including, but not limited to, determining the validity of this Agreement, the meaning of its
' provisions and the rights, obligations and remedies of the parties.

' D. Entire Understanding: This Agreement constitutes the entire understanding and agreement of the
' parties relating to release of the Subject Software and may not be superseded, modified or amended
' except by further written agreement duly executed by the parties.

' E. Binding Authority: By accepting and using the Subject Software under this Agreement, a Recipient
' affirms its authority to bind the Recipient to all terms and conditions of this Agreement and that
' Recipient hereby agrees to all terms and conditions herein.

' F. Point of Contact: Any Recipient contact with Government Agency is to be directed to the designated
' representative as follows: J. Ritchie Carroll <jrcarrol@tva.gov>.

#End Region

Imports EnvDTE
Imports EnvDTE80
Imports EnvDTE90
Imports System
Imports System.Diagnostics
Imports System.DirectoryServices
Imports System.Security.Principal
Imports System.Text
Imports Microsoft.Win32

Public Module ProjectMacros

    Private m_userEntry As DirectoryEntry

    Public Sub InsertHeader()

        Dim headerText = New StringBuilder()

        With headerText
            .AppendLine("//*******************************************************************************************************")
            .AppendLine("//  " & DTE.ActiveWindow.ProjectItem.Name & " - Gbtc")
            .AppendLine("//")
            .AppendLine("//  Tennessee Valley Authority, " & Date.Now.Year)
            .AppendLine("//  No copyright is claimed pursuant to 17 USC § 105.  All Other Rights Reserved.")
            .AppendLine("//")
            .AppendLine("//  This software is made freely available under the TVA Open Source Agreement (see below).")
            .AppendLine("//")
            .AppendLine("//  Code Modification History:")
            .AppendLine("//  -----------------------------------------------------------------------------------------------------")
            .AppendLine("//  " & DateTime.Now.ToShortDateString() & " - " & FullName)
            .AppendLine("//       Generated original version of source code.")
            .AppendLine("//")
            .AppendLine("//*******************************************************************************************************")
            .AppendLine()
            .AppendLine("#region [ TVA Open Source Agreement ]")
            .AppendLine("/*")
            .AppendLine()
            .AppendLine(" THIS OPEN SOURCE AGREEMENT (""AGREEMENT"") DEFINES THE RIGHTS OF USE,REPRODUCTION, DISTRIBUTION,")
            .AppendLine(" MODIFICATION AND REDISTRIBUTION OF CERTAIN COMPUTER SOFTWARE ORIGINALLY RELEASED BY THE")
            .AppendLine(" TENNESSEE VALLEY AUTHORITY, A CORPORATE AGENCY AND INSTRUMENTALITY OF THE UNITED STATES GOVERNMENT")
            .AppendLine(" (""GOVERNMENT AGENCY""). GOVERNMENT AGENCY IS AN INTENDED THIRD-PARTY BENEFICIARY OF ALL SUBSEQUENT")
            .AppendLine(" DISTRIBUTIONS OR REDISTRIBUTIONS OF THE SUBJECT SOFTWARE. ANYONE WHO USES, REPRODUCES, DISTRIBUTES,")
            .AppendLine(" MODIFIES OR REDISTRIBUTES THE SUBJECT SOFTWARE, AS DEFINED HEREIN, OR ANY PART THEREOF, IS, BY THAT")
            .AppendLine(" ACTION, ACCEPTING IN FULL THE RESPONSIBILITIES AND OBLIGATIONS CONTAINED IN THIS AGREEMENT.")
            .AppendLine()
            .AppendLine(" Original Software Designation: openPDC")
            .AppendLine(" Original Software Title: The TVA Open Source Phasor Data Concentrator")
            .AppendLine(" User Registration Requested. Please Visit https://naspi.tva.com/Registration/")
            .AppendLine(" Point of Contact for Original Software: J. Ritchie Carroll <jrcarrol@tva.gov>")
            .AppendLine()
            .AppendLine(" 1. DEFINITIONS")
            .AppendLine()
            .AppendLine(" A. ""Contributor"" means Government Agency, as the developer of the Original Software, and any entity")
            .AppendLine(" that makes a Modification.")
            .AppendLine()
            .AppendLine(" B. ""Covered Patents"" mean patent claims licensable by a Contributor that are necessarily infringed by")
            .AppendLine(" the use or sale of its Modification alone or when combined with the Subject Software.")
            .AppendLine()
            .AppendLine(" C. ""Display"" means the showing of a copy of the Subject Software, either directly or by means of an")
            .AppendLine(" image, or any other device.")
            .AppendLine()
            .AppendLine(" D. ""Distribution"" means conveyance or transfer of the Subject Software, regardless of means, to")
            .AppendLine(" another.")
            .AppendLine()
            .AppendLine(" E. ""Larger Work"" means computer software that combines Subject Software, or portions thereof, with")
            .AppendLine(" software separate from the Subject Software that is not governed by the terms of this Agreement.")
            .AppendLine()
            .AppendLine(" F. ""Modification"" means any alteration of, including addition to or deletion from, the substance or")
            .AppendLine(" structure of either the Original Software or Subject Software, and includes derivative works, as that")
            .AppendLine(" term is defined in the Copyright Statute, 17 USC § 101. However, the act of including Subject Software")
            .AppendLine(" as part of a Larger Work does not in and of itself constitute a Modification.")
            .AppendLine()
            .AppendLine(" G. ""Original Software"" means the computer software first released under this Agreement by Government")
            .AppendLine(" Agency entitled openPDC, including source code, object code and accompanying documentation, if any.")
            .AppendLine()
            .AppendLine(" H. ""Recipient"" means anyone who acquires the Subject Software under this Agreement, including all")
            .AppendLine(" Contributors.")
            .AppendLine()
            .AppendLine(" I. ""Redistribution"" means Distribution of the Subject Software after a Modification has been made.")
            .AppendLine()
            .AppendLine(" J. ""Reproduction"" means the making of a counterpart, image or copy of the Subject Software.")
            .AppendLine()
            .AppendLine(" K. ""Sale"" means the exchange of the Subject Software for money or equivalent value.")
            .AppendLine()
            .AppendLine(" L. ""Subject Software"" means the Original Software, Modifications, or any respective parts thereof.")
            .AppendLine()
            .AppendLine(" M. ""Use"" means the application or employment of the Subject Software for any purpose.")
            .AppendLine()
            .AppendLine(" 2. GRANT OF RIGHTS")
            .AppendLine()
            .AppendLine(" A. Under Non-Patent Rights: Subject to the terms and conditions of this Agreement, each Contributor,")
            .AppendLine(" with respect to its own contribution to the Subject Software, hereby grants to each Recipient a")
            .AppendLine(" non-exclusive, world-wide, royalty-free license to engage in the following activities pertaining to")
            .AppendLine(" the Subject Software:")
            .AppendLine()
            .AppendLine(" 1. Use")
            .AppendLine()
            .AppendLine(" 2. Distribution")
            .AppendLine()
            .AppendLine(" 3. Reproduction")
            .AppendLine()
            .AppendLine(" 4. Modification")
            .AppendLine()
            .AppendLine(" 5. Redistribution")
            .AppendLine()
            .AppendLine(" 6. Display")
            .AppendLine()
            .AppendLine(" B. Under Patent Rights: Subject to the terms and conditions of this Agreement, each Contributor, with")
            .AppendLine(" respect to its own contribution to the Subject Software, hereby grants to each Recipient under Covered")
            .AppendLine(" Patents a non-exclusive, world-wide, royalty-free license to engage in the following activities")
            .AppendLine(" pertaining to the Subject Software:")
            .AppendLine()
            .AppendLine(" 1. Use")
            .AppendLine()
            .AppendLine(" 2. Distribution")
            .AppendLine()
            .AppendLine(" 3. Reproduction")
            .AppendLine()
            .AppendLine(" 4. Sale")
            .AppendLine()
            .AppendLine(" 5. Offer for Sale")
            .AppendLine()
            .AppendLine(" C. The rights granted under Paragraph B. also apply to the combination of a Contributor's Modification")
            .AppendLine(" and the Subject Software if, at the time the Modification is added by the Contributor, the addition of")
            .AppendLine(" such Modification causes the combination to be covered by the Covered Patents. It does not apply to")
            .AppendLine(" any other combinations that include a Modification. ")
            .AppendLine()
            .AppendLine(" D. The rights granted in Paragraphs A. and B. allow the Recipient to sublicense those same rights.")
            .AppendLine(" Such sublicense must be under the same terms and conditions of this Agreement.")
            .AppendLine()
            .AppendLine(" 3. OBLIGATIONS OF RECIPIENT")
            .AppendLine()
            .AppendLine(" A. Distribution or Redistribution of the Subject Software must be made under this Agreement except for")
            .AppendLine(" additions covered under paragraph 3H. ")
            .AppendLine()
            .AppendLine(" 1. Whenever a Recipient distributes or redistributes the Subject Software, a copy of this Agreement")
            .AppendLine(" must be included with each copy of the Subject Software; and")
            .AppendLine()
            .AppendLine(" 2. If Recipient distributes or redistributes the Subject Software in any form other than source code,")
            .AppendLine(" Recipient must also make the source code freely available, and must provide with each copy of the")
            .AppendLine(" Subject Software information on how to obtain the source code in a reasonable manner on or through a")
            .AppendLine(" medium customarily used for software exchange.")
            .AppendLine()
            .AppendLine(" B. Each Recipient must ensure that the following copyright notice appears prominently in the Subject")
            .AppendLine(" Software:")
            .AppendLine()
            .AppendLine("          No copyright is claimed pursuant to 17 USC § 105.  All Other Rights Reserved.")
            .AppendLine()
            .AppendLine(" C. Each Contributor must characterize its alteration of the Subject Software as a Modification and")
            .AppendLine(" must identify itself as the originator of its Modification in a manner that reasonably allows")
            .AppendLine(" subsequent Recipients to identify the originator of the Modification. In fulfillment of these")
            .AppendLine(" requirements, Contributor must include a file (e.g., a change log file) that describes the alterations")
            .AppendLine(" made and the date of the alterations, identifies Contributor as originator of the alterations, and")
            .AppendLine(" consents to characterization of the alterations as a Modification, for example, by including a")
            .AppendLine(" statement that the Modification is derived, directly or indirectly, from Original Software provided by")
            .AppendLine(" Government Agency. Once consent is granted, it may not thereafter be revoked.")
            .AppendLine()
            .AppendLine(" D. A Contributor may add its own copyright notice to the Subject Software. Once a copyright notice has")
            .AppendLine(" been added to the Subject Software, a Recipient may not remove it without the express permission of")
            .AppendLine(" the Contributor who added the notice.")
            .AppendLine()
            .AppendLine(" E. A Recipient may not make any representation in the Subject Software or in any promotional,")
            .AppendLine(" advertising or other material that may be construed as an endorsement by Government Agency or by any")
            .AppendLine(" prior Recipient of any product or service provided by Recipient, or that may seek to obtain commercial")
            .AppendLine(" advantage by the fact of Government Agency's or a prior Recipient's participation in this Agreement.")
            .AppendLine()
            .AppendLine(" F. In an effort to track usage and maintain accurate records of the Subject Software, each Recipient,")
            .AppendLine(" upon receipt of the Subject Software, is requested to register with Government Agency by visiting the")
            .AppendLine(" following website: https://naspi.tva.com/Registration/. Recipient's name and personal information")
            .AppendLine(" shall be used for statistical purposes only. Once a Recipient makes a Modification available, it is")
            .AppendLine(" requested that the Recipient inform Government Agency at the web site provided above how to access the")
            .AppendLine(" Modification.")
            .AppendLine()
            .AppendLine(" G. Each Contributor represents that that its Modification does not violate any existing agreements,")
            .AppendLine(" regulations, statutes or rules, and further that Contributor has sufficient rights to grant the rights")
            .AppendLine(" conveyed by this Agreement.")
            .AppendLine()
            .AppendLine(" H. A Recipient may choose to offer, and to charge a fee for, warranty, support, indemnity and/or")
            .AppendLine(" liability obligations to one or more other Recipients of the Subject Software. A Recipient may do so,")
            .AppendLine(" however, only on its own behalf and not on behalf of Government Agency or any other Recipient. Such a")
            .AppendLine(" Recipient must make it absolutely clear that any such warranty, support, indemnity and/or liability")
            .AppendLine(" obligation is offered by that Recipient alone. Further, such Recipient agrees to indemnify Government")
            .AppendLine(" Agency and every other Recipient for any liability incurred by them as a result of warranty, support,")
            .AppendLine(" indemnity and/or liability offered by such Recipient.")
            .AppendLine()
            .AppendLine(" I. A Recipient may create a Larger Work by combining Subject Software with separate software not")
            .AppendLine(" governed by the terms of this agreement and distribute the Larger Work as a single product. In such")
            .AppendLine(" case, the Recipient must make sure Subject Software, or portions thereof, included in the Larger Work")
            .AppendLine(" is subject to this Agreement.")
            .AppendLine()
            .AppendLine(" J. Notwithstanding any provisions contained herein, Recipient is hereby put on notice that export of")
            .AppendLine(" any goods or technical data from the United States may require some form of export license from the")
            .AppendLine(" U.S. Government. Failure to obtain necessary export licenses may result in criminal liability under")
            .AppendLine(" U.S. laws. Government Agency neither represents that a license shall not be required nor that, if")
            .AppendLine(" required, it shall be issued. Nothing granted herein provides any such export license.")
            .AppendLine()
            .AppendLine(" 4. DISCLAIMER OF WARRANTIES AND LIABILITIES; WAIVER AND INDEMNIFICATION")
            .AppendLine()
            .AppendLine(" A. No Warranty: THE SUBJECT SOFTWARE IS PROVIDED ""AS IS"" WITHOUT ANY WARRANTY OF ANY KIND, EITHER")
            .AppendLine(" EXPRESSED, IMPLIED, OR STATUTORY, INCLUDING, BUT NOT LIMITED TO, ANY WARRANTY THAT THE SUBJECT")
            .AppendLine(" SOFTWARE WILL CONFORM TO SPECIFICATIONS, ANY IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A")
            .AppendLine(" PARTICULAR PURPOSE, OR FREEDOM FROM INFRINGEMENT, ANY WARRANTY THAT THE SUBJECT SOFTWARE WILL BE ERROR")
            .AppendLine(" FREE, OR ANY WARRANTY THAT DOCUMENTATION, IF PROVIDED, WILL CONFORM TO THE SUBJECT SOFTWARE. THIS")
            .AppendLine(" AGREEMENT DOES NOT, IN ANY MANNER, CONSTITUTE AN ENDORSEMENT BY GOVERNMENT AGENCY OR ANY PRIOR")
            .AppendLine(" RECIPIENT OF ANY RESULTS, RESULTING DESIGNS, HARDWARE, SOFTWARE PRODUCTS OR ANY OTHER APPLICATIONS")
            .AppendLine(" RESULTING FROM USE OF THE SUBJECT SOFTWARE. FURTHER, GOVERNMENT AGENCY DISCLAIMS ALL WARRANTIES AND")
            .AppendLine(" LIABILITIES REGARDING THIRD-PARTY SOFTWARE, IF PRESENT IN THE ORIGINAL SOFTWARE, AND DISTRIBUTES IT")
            .AppendLine(" ""AS IS.""")
            .AppendLine()
            .AppendLine(" B. Waiver and Indemnity: RECIPIENT AGREES TO WAIVE ANY AND ALL CLAIMS AGAINST GOVERNMENT AGENCY, ITS")
            .AppendLine(" AGENTS, EMPLOYEES, CONTRACTORS AND SUBCONTRACTORS, AS WELL AS ANY PRIOR RECIPIENT. IF RECIPIENT'S USE")
            .AppendLine(" OF THE SUBJECT SOFTWARE RESULTS IN ANY LIABILITIES, DEMANDS, DAMAGES, EXPENSES OR LOSSES ARISING FROM")
            .AppendLine(" SUCH USE, INCLUDING ANY DAMAGES FROM PRODUCTS BASED ON, OR RESULTING FROM, RECIPIENT'S USE OF THE")
            .AppendLine(" SUBJECT SOFTWARE, RECIPIENT SHALL INDEMNIFY AND HOLD HARMLESS  GOVERNMENT AGENCY, ITS AGENTS,")
            .AppendLine(" EMPLOYEES, CONTRACTORS AND SUBCONTRACTORS, AS WELL AS ANY PRIOR RECIPIENT, TO THE EXTENT PERMITTED BY")
            .AppendLine(" LAW.  THE FOREGOING RELEASE AND INDEMNIFICATION SHALL APPLY EVEN IF THE LIABILITIES, DEMANDS, DAMAGES,")
            .AppendLine(" EXPENSES OR LOSSES ARE CAUSED, OCCASIONED, OR CONTRIBUTED TO BY THE NEGLIGENCE, SOLE OR CONCURRENT, OF")
            .AppendLine(" GOVERNMENT AGENCY OR ANY PRIOR RECIPIENT.  RECIPIENT'S SOLE REMEDY FOR ANY SUCH MATTER SHALL BE THE")
            .AppendLine(" IMMEDIATE, UNILATERAL TERMINATION OF THIS AGREEMENT.")
            .AppendLine()
            .AppendLine(" 5. GENERAL TERMS")
            .AppendLine()
            .AppendLine(" A. Termination: This Agreement and the rights granted hereunder will terminate automatically if a")
            .AppendLine(" Recipient fails to comply with these terms and conditions, and fails to cure such noncompliance within")
            .AppendLine(" thirty (30) days of becoming aware of such noncompliance. Upon termination, a Recipient agrees to")
            .AppendLine(" immediately cease use and distribution of the Subject Software. All sublicenses to the Subject")
            .AppendLine(" Software properly granted by the breaching Recipient shall survive any such termination of this")
            .AppendLine(" Agreement.")
            .AppendLine()
            .AppendLine(" B. Severability: If any provision of this Agreement is invalid or unenforceable under applicable law,")
            .AppendLine(" it shall not affect the validity or enforceability of the remainder of the terms of this Agreement.")
            .AppendLine()
            .AppendLine(" C. Applicable Law: This Agreement shall be subject to United States federal law only for all purposes,")
            .AppendLine(" including, but not limited to, determining the validity of this Agreement, the meaning of its")
            .AppendLine(" provisions and the rights, obligations and remedies of the parties.")
            .AppendLine()
            .AppendLine(" D. Entire Understanding: This Agreement constitutes the entire understanding and agreement of the")
            .AppendLine(" parties relating to release of the Subject Software and may not be superseded, modified or amended")
            .AppendLine(" except by further written agreement duly executed by the parties.")
            .AppendLine()
            .AppendLine(" E. Binding Authority: By accepting and using the Subject Software under this Agreement, a Recipient")
            .AppendLine(" affirms its authority to bind the Recipient to all terms and conditions of this Agreement and that")
            .AppendLine(" Recipient hereby agrees to all terms and conditions herein.")
            .AppendLine()
            .AppendLine(" F. Point of Contact: Any Recipient contact with Government Agency is to be directed to the designated")
            .AppendLine(" representative as follows: J. Ritchie Carroll <jrcarrol@tva.gov>.")
            .AppendLine()
            .AppendLine("*/")
            .AppendLine("#endregion")
            .AppendLine()
        End With

        With DTE.ActiveDocument.Selection
            .StartOfDocument(False)
            .Insert(headerText.ToString(), vsInsertFlags.vsInsertFlagsCollapseToEnd)
        End With

    End Sub

    Public Sub FormatAll()

        Dim project As Project
        Dim projectObjects As Object()
        Dim window As Window
        Dim target As Object

        window = DTE.Windows.Item(Constants.vsWindowKindCommandWindow)
        projectObjects = DTE.ActiveSolutionProjects

        If projectObjects.Length = 0 Then
            Exit Sub
        End If

        project = DTE.ActiveSolutionProjects(0)

        If (DTE.ActiveWindow Is window) Then
            target = window.Object
        Else
            target = GetOutputWindowPane("List Project")
            target.Clear()
        End If

        RecurseProjectFolders(project.ProjectItems(), 0, target)

    End Sub

    Private Sub RecurseProjectFolders(ByVal projectItems As EnvDTE.ProjectItems, ByVal level As Integer, ByVal outputWinPane As Object)

        Dim projectItem As EnvDTE.ProjectItem

        For Each projectItem In projectItems
            ' Ignore item if it is not rooted in this collection (check for VC project model).
            If projectItem.Collection Is projectItems Then
                ' Execute formatting action
                PerformCodeItem(projectItem, level, outputWinPane)
                ' Recurse if this item has subitems ...
                Dim projectItems2 As EnvDTE.ProjectItems = projectItem.ProjectItems
                If projectItems2 IsNot Nothing Then RecurseProjectFolders(projectItems2, level + 1, outputWinPane)
            End If
        Next

    End Sub

    Private Sub PerformCodeItem(ByVal projectItem As EnvDTE.ProjectItem, ByVal level As Integer, ByVal outputWinPane As Object)

        Dim window As EnvDTE.Window
        Dim alreadyOpen As Boolean

        If projectItem.Name.EndsWith(".cs") Then
            alreadyOpen = projectItem.IsOpen(Constants.vsext_vk_Code)
            window = projectItem.Open(Constants.vsext_vk_Code)
            window.Activate()
            DTE.ExecuteCommand("Edit.FormatDocument")
            If Not alreadyOpen Then window.Close(vsSaveChanges.vsSaveChangesYes)
        End If

    End Sub

    Private Function GetOutputWindowPane(ByVal Name As String, Optional ByVal show As Boolean = True) As OutputWindowPane

        Dim window As Window
        Dim outputWindow As OutputWindow
        Dim outputWindowPane As OutputWindowPane

        window = DTE.Windows.Item(EnvDTE.Constants.vsWindowKindOutput)
        If show Then window.Visible = True
        outputWindow = window.Object

        Try
            outputWindowPane = outputWindow.OutputWindowPanes.Item(Name)
        Catch e As System.Exception
            outputWindowPane = outputWindow.OutputWindowPanes.Add(Name)
        End Try

        outputWindowPane.Activate()

        Return outputWindowPane

    End Function

    Private ReadOnly Property FullName() As String
        Get
            ' If machine name and domain are the same, user is likely not logged into a domain so
            ' there's a good probablility that no active directory services will be available...
            If String.Compare(Environment.MachineName.Trim(), Environment.UserDomainName.Trim(), True) = 0 Then
                ' If not running on a domain, we use user name from Visual Studio registration
                Dim registeredName As String = Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\9.0\Registration", "UserName", "")

                If String.IsNullOrEmpty(registeredName) Then
                    Return Environment.UserName
                Else
                    Return registeredName
                End If
            Else
                ' Otherwise we get name defined in ActiveDirectory for logged in user
                If Not String.IsNullOrEmpty(FirstName) AndAlso Not String.IsNullOrEmpty(LastName) Then
                    If String.IsNullOrEmpty(MiddleInitial) Then
                        Return FirstName & " " & LastName
                    Else
                        Return FirstName & " " & MiddleInitial & ". " & LastName
                    End If
                Else
                    Return Environment.UserName
                End If
            End If
        End Get
    End Property

    Private ReadOnly Property UserEntry() As DirectoryEntry
        Get
            If m_userEntry Is Nothing Then
                Try
                    Dim entry As New DirectoryEntry()
                    With New DirectorySearcher(entry)
                        .Filter = "(SAMAccountName=" & Environment.UserName & ")"
                        m_userEntry = .FindOne().GetDirectoryEntry()
                    End With
                Catch
                    m_userEntry = Nothing
                    Throw
                End Try
            End If
            Return m_userEntry
        End Get
    End Property

    Private ReadOnly Property UserProperty(ByVal propertyName As System.String) As String
        Get
            Try
                Return UserEntry.Properties(propertyName)(0).ToString().Replace("  ", " ").Trim()
            Catch
                Return ""
            End Try
        End Get
    End Property

    Private ReadOnly Property FirstName() As String
        Get
            Return UserProperty("givenName")
        End Get
    End Property

    Private ReadOnly Property LastName() As String
        Get
            Return UserProperty("sn")
        End Get
    End Property

    Private ReadOnly Property MiddleInitial() As String
        Get
            Return UserProperty("initials")
        End Get
    End Property

    Private ReadOnly Property Email() As String
        Get
            Return UserProperty("mail")
        End Get
    End Property

    Private ReadOnly Property Telephone() As String
        Get
            Return UserProperty("telephoneNumber")
        End Get
    End Property

    Private ReadOnly Property Title() As String
        Get
            Return UserProperty("title")
        End Get
    End Property

    Private ReadOnly Property Company() As String
        Get
            Return UserProperty("company")
        End Get
    End Property

    Private ReadOnly Property Office() As String
        Get
            Return UserProperty("physicalDeliveryOfficeName")
        End Get
    End Property

    Private ReadOnly Property Department() As String
        Get
            Return UserProperty("department")
        End Get
    End Property

    Private ReadOnly Property City() As String
        Get
            Return UserProperty("l")
        End Get
    End Property

    Private ReadOnly Property Mailbox() As String
        Get
            Return UserProperty("streetAddress")
        End Get
    End Property

    Public Sub XmlCodeCommentRegion()

        Dim selection As EnvDTE.TextSelection
        Dim startPoint As EnvDTE.EditPoint
        Dim endPoint As TextPoint
        Dim commentStart As String

        selection = DTE.ActiveDocument.Selection()
        startPoint = selection.TopPoint.CreateEditPoint()
        endPoint = selection.BottomPoint
        commentStart = LineOrientedXmlCodeCommentStart()
        DTE.UndoContext.Open("Xml Code Comment Region")

        Try
            Do While (True)
                Dim line As Integer
                line = startPoint.Line
                startPoint.Insert(commentStart)
                startPoint.LineDown()
                startPoint.StartOfLine()
                If (line = endPoint.Line) Then Exit Do
            Loop
        Finally
            ' If an error occurred, then make sure that the undo context is cleaned up.
            ' Otherwise, the editor can be left in a perpetual undo context.
            DTE.UndoContext.Close()
        End Try

    End Sub

    Private Function LineOrientedXmlCodeCommentStart(Optional ByVal document As Document = Nothing) As String

        Dim extension As String

        If document Is Nothing Then document = DTE.ActiveDocument

        extension = document.Name

        If (extension.EndsWith(".cs") Or extension.EndsWith(".cpp") Or extension.EndsWith(".h") Or extension.EndsWith(".idl") Or extension.EndsWith(".jsl")) Then
            Return "/// "
        ElseIf (extension.EndsWith(".vb")) Then
            Return "''' "
        Else
            Throw New Exception("Unrecognized file type. You can add this file type by modifying the function LineOrientedXmlCodeCommentStart to include the extension of this file.")
        End If

    End Function

End Module
