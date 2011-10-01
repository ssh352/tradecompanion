#define WRSTHNK_EXP __declspec(dllimport)
WRSTHNK_EXP DWORD StartServer(HWND m_hWnd,DWORD*errorCode);
WRSTHNK_EXP UINT SendTrade(LPCTSTR ticker,DWORD trade,DWORD open,
							DWORD high,DWORD low, DWORD change,DWORD prevClose,
							DWORD base,
							DWORD tvolume, DWORD cvolume,
							UINT year,UINT month,UINT day,
							UINT hour,UINT minute,UINT second,DWORD*errorCode);
WRSTHNK_EXP UINT SendOrder(LPCTSTR ticker,DWORD ask,DWORD bid,
							DWORD base,
							DWORD askSize, DWORD bidSize,
							UINT year,UINT month,UINT day,
							UINT hour,UINT minute,UINT second,DWORD*errorCode);

WRSTHNK_EXP BOOL StopServer(DWORD*errorCode);