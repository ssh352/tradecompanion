// TestDlg.cpp : implementation file
//

#include "stdafx.h"
#include "Test.h"
#include "TestDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CAboutDlg dialog used for App About

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// Dialog Data
	//{{AFX_DATA(CAboutDlg)
	enum { IDD = IDD_ABOUTBOX };
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAboutDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	//{{AFX_MSG(CAboutDlg)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
	//{{AFX_DATA_INIT(CAboutDlg)
	//}}AFX_DATA_INIT
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAboutDlg)
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
	//{{AFX_MSG_MAP(CAboutDlg)
		// No message handlers
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTestDlg dialog

CTestDlg::CTestDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CTestDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CTestDlg)
	m_cvol = 0;
	m_date = COleDateTime::GetCurrentTime();
	m_incvol = 0;
	m_trade = 0;
	m_high = 0;
	m_low = 0;
	m_open = 0;
	m_symbol = _T("");
	m_rand = FALSE;
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CTestDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTestDlg)
	DDX_Control(pDX, IDC_RANDFREQ, m_sl);
	DDX_Text(pDX, IDC_CUMVOL, m_cvol);
	DDX_Text(pDX, IDC_DATETIME, m_date);
	DDX_Text(pDX, IDC_INCVOL, m_incvol);
	DDX_Text(pDX, IDC_TRADE, m_trade);
	DDX_Text(pDX, IDC_HIGH, m_high);
	DDX_Text(pDX, IDC_LOW, m_low);
	DDX_Text(pDX, IDC_OPEN, m_open);
	DDX_Text(pDX, IDC_SYMBOL, m_symbol);
	DDX_Check(pDX, IDC_CHECKRAND, m_rand);
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CTestDlg, CDialog)
	//{{AFX_MSG_MAP(CTestDlg)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_START, OnStart)
	ON_BN_CLICKED(IDC_STOP, OnStop)
	ON_BN_CLICKED(IDC_SEND, OnSend)
	ON_WM_TIMER()
	ON_BN_CLICKED(IDC_CHECKRAND, OnCheckrand)
	ON_NOTIFY(NM_RELEASEDCAPTURE, IDC_RANDFREQ, OnRandfreq)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTestDlg message handlers

BOOL CTestDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	m_sl.SetRangeMax(10000);
	srand( (unsigned)time( NULL ) );
	
	m_symbol="TEST";
	UpdateData(FALSE);
	
	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CTestDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CTestDlg::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// The system calls this to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CTestDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}

void CTestDlg::OnStart() 
{
	DWORD errorCode;
	threadId=StartServer(m_hWnd,&errorCode);
}

void CTestDlg::OnStop() 
{
	DWORD errorCode;
	StopServer(&errorCode);
}

void CTestDlg::OnSend() 
{
	UpdateData(TRUE);
	DWORD errorCode;
	SendTrade(m_symbol,m_trade,m_open,m_high,m_low,m_trade-m_open,m_open,10,
				m_incvol, m_cvol,
				m_date.GetYear(),m_date.GetMonth(),m_date.GetDay(),
				m_date.GetHour(),m_date.GetMinute(),m_date.GetSecond(),&errorCode);
	Sleep(50);
	SendOrder(m_symbol,m_trade,m_open,10,
				m_incvol, m_cvol,
				m_date.GetYear(),m_date.GetMonth(),m_date.GetDay(),
				m_date.GetHour(),m_date.GetMinute(),m_date.GetSecond(),&errorCode);
	Sleep(50);

	m_date=COleDateTime::GetCurrentTime();
	UpdateData(FALSE);


}

void CTestDlg::OnTimer(UINT nIDEvent) 
{
	SendRandom();
	
	CDialog::OnTimer(nIDEvent);
}

void CTestDlg::OnCheckrand() 
{
	UpdateData(TRUE);	
	if(m_rand==1){
		m_sl.EnableWindow(TRUE);
		int pos=m_sl.GetPos();
		if(pos>0){
			SetTimer(1,pos,NULL);
		}
	}
	else{
		m_sl.EnableWindow(FALSE);
		KillTimer(1);
	}
}

void CTestDlg::OnRandfreq(NMHDR* pNMHDR, LRESULT* pResult) 
{
	int pos=m_sl.GetPos();
	KillTimer(1);
	SetTimer(1,pos,NULL);
	
	*pResult = 0;
}

void CTestDlg::SendRandom()
{

	m_trade=rand();
	m_low=m_trade-1;
	m_high=m_trade+10;
	m_open=m_trade+1;
	m_incvol=m_trade*10;
	m_cvol=m_trade*20;
	UpdateData(FALSE);
	OnSend();

}
