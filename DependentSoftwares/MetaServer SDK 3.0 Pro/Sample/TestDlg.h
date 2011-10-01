// TestDlg.h : header file
//

#if !defined(AFX_TESTDLG_H__950E23C6_67F4_11D4_BB55_006097D2E6FA__INCLUDED_)
#define AFX_TESTDLG_H__950E23C6_67F4_11D4_BB55_006097D2E6FA__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/////////////////////////////////////////////////////////////////////////////
// CTestDlg dialog

class CTestDlg : public CDialog
{
// Construction
public:
	void SendRandom();
	DWORD threadId;
	CTestDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	//{{AFX_DATA(CTestDlg)
	enum { IDD = IDD_TEST_DIALOG };
	CSliderCtrl	m_sl;
	UINT	m_cvol;
	COleDateTime	m_date;
	UINT	m_incvol;
	UINT	m_trade;
	UINT	m_high;
	UINT	m_low;
	UINT	m_open;
	CString	m_symbol;
	BOOL	m_rand;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTestDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	//{{AFX_MSG(CTestDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnStart();
	afx_msg void OnStop();
	afx_msg void OnSend();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg void OnCheckrand();
	afx_msg void OnRandfreq(NMHDR* pNMHDR, LRESULT* pResult);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TESTDLG_H__950E23C6_67F4_11D4_BB55_006097D2E6FA__INCLUDED_)
