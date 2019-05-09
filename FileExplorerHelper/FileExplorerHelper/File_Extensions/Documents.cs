using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorerHelper
{
    public enum Documents
    {
        // compressed files
        ARJ, DEB, PKG, RAR, RPM, GZ, Z, ZIP,
        // database
        CSV, DAT, DB, DBF, LOG, MDB, SAV, SQL, TAR, XML,
        // fonts
        FNT, FON, OTF, TTF,
        // internet
        ASP, ASPX, CER, CFM, CGI, CSS, HTM, HTML, JS, JSP, PART, PHP, PY, RSS, XHTML,
        // presentation
        KEY, OPD, PPS, PPT, PPTX,
        // programming files
        C, CLASS, CPP, CS, H, JAVA, SH, SWIFT, VB,
        // spreadsheet
        ODS, XLR, XLS, XLSX,
        // word processing
        DOC, DOCX, ODT, PDF, RTF, TEX, TXT, WKS, WPS, WPD
    }
}


