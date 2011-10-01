package fxintegrate.demo.download;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileInputStream;
import java.io.FileReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.URL;
import java.security.KeyStore;
import java.security.KeyStoreException;
import java.security.cert.X509Certificate;
import java.util.MissingResourceException;
import java.util.PropertyResourceBundle;

import javax.net.ssl.SSLSocketFactory;

import com.sun.net.ssl.HttpsURLConnection;
import com.sun.net.ssl.KeyManagerFactory;
import com.sun.net.ssl.SSLContext;
import com.sun.net.ssl.TrustManager;
import com.sun.net.ssl.X509TrustManager;

import fxintegrate.download.Constants;
import fxintegrate.util.FileUtil;
import fxintegrate.util.X509ServerTrustManager;

public class DownloadClient {

    // data

    private static final int DEBUG_LEVEL_ = 1;

    private static final String DEFAULT_DOWNLOAD_PROPERTIES_FILENAME_ = "download.properties";

    private static final int DEFAULT_SSL_LISTEN_PORT_ = 443;

    // static initializer

    static {

	System.getProperties().put( "java.protocol.handler.pkgs",
				    "com.sun.net.ssl.internal.www.protocol" );
    }

    // methods

    private void doDownload_( ParamInfo_ aParamInfo ) throws Exception {

	BufferedReader in;
	HttpsURLConnection con;
	int port;
	PrintWriter out;
	SSLSocketFactory sslSocketFactory;
	String downloadRequestXML;
	String host;
	String inputLine;
	String keystorePassword;
	String keystorePath;
	String path;
	String xsltStylesheet;
	URL url;

	keystorePath = aParamInfo.getKeystorePath();
	keystorePassword = aParamInfo.getKeystorePassword();
	host = aParamInfo.getHost();
	port = aParamInfo.getPort();
	path = aParamInfo.getPath();
	downloadRequestXML = aParamInfo.getDownloadRequestXML();
	xsltStylesheet = aParamInfo.getXSLTStylesheet();

	debugPrint_( 3, "doDownload_(): downloadRequestXML: \n" + downloadRequestXML );

	// get connection to Currenex Download Server

	if( path.charAt( 0 ) != '/' ) {

	    path = '/' + path;
	}

	url = new URL( "https://" + host + ':' + port + path );

	debugPrint_( 3, "doDownload_(): url: " + url );

	con = ( HttpsURLConnection ) url.openConnection();

	con.setDoOutput( true );

	sslSocketFactory = getSSLSocketFactory_( keystorePath,
						 keystorePassword );

	con.setSSLSocketFactory( sslSocketFactory );

	out =
	    new PrintWriter( new BufferedWriter( new OutputStreamWriter( con.getOutputStream() ) ) );

	downloadRequestXML = java.net.URLEncoder.encode( downloadRequestXML );

	out.write( Constants.KEY_DOWNLOAD_REQUEST_XML + '=' +
		   downloadRequestXML );

	if( xsltStylesheet != null ) {

	    xsltStylesheet = java.net.URLEncoder.encode( xsltStylesheet );

	    out.write( '&' + Constants.KEY_XSLT_STYLESHEET + '=' +
		       xsltStylesheet );
	}

	out.flush();

	out.close();

	// read, display response

	in = new BufferedReader( new InputStreamReader( con.getInputStream() ) );

	while( ( inputLine = in.readLine() ) != null ) {

	    System.out.println( inputLine );
	}

	in.close();
    }

    private SSLSocketFactory getSSLSocketFactory_( String aKeystorePath,
						   String aKeystorePassword )
	throws Exception {

	KeyManagerFactory keyManagerFactory;
	KeyStore keyStore;
	SSLContext sslContext;
	TrustManager trustManagerList[];

	// Set up a key manager for client authentication if asked by the
	// server.

	keyStore = KeyStore.getInstance( "JKS" );

	keyStore.load( new FileInputStream( aKeystorePath ),
		       aKeystorePassword.toCharArray() );

	keyManagerFactory = KeyManagerFactory.getInstance( "SunX509" );

	keyManagerFactory.init( keyStore, aKeystorePassword.toCharArray() );

	trustManagerList = getTrustManagerList_( keyStore );

	sslContext = SSLContext.getInstance( "TLS" );

	sslContext.init( keyManagerFactory.getKeyManagers(), trustManagerList,
			 null );

	return sslContext.getSocketFactory();
    }

    private TrustManager[] getTrustManagerList_( KeyStore aKeyStore ) 
	throws KeyStoreException {

	TrustManager trustManagerList[];
	X509TrustManager trustManager;

	trustManager = new X509ServerTrustManager( aKeyStore );

	trustManagerList = new TrustManager[ 1 ];
	trustManagerList[ 0 ] = trustManager;

	return trustManagerList;
    }

    private static void debugPrint_( int aLevel, String aStr ) {

	if( aLevel <= DEBUG_LEVEL_ ) {

	    System.out.println( "DownloadClient: " + aStr );
	}
    }

    public static void main( String[] aArgs ) {

	DownloadClient client;
	ParamInfo_ paramInfo;
	String downloadPropertiesFilename = null;

	try {

	    // get parameters from download properties file; the filename
	    // defaults to "download.properties" if it's not specified on the
	    // command line. The corresponding file must reside in the system
	    // classpath

	    downloadPropertiesFilename = ( aArgs.length > 0 ) ?
		aArgs[ 0 ] : DEFAULT_DOWNLOAD_PROPERTIES_FILENAME_;

	    paramInfo = new ParamInfo_( downloadPropertiesFilename );

	    client = new DownloadClient();

	    client.doDownload_( paramInfo );
	}
	catch( Exception e ) {

	    debugPrint_( 1, "error occurred: " + e.getMessage() );

	    debugPrint_( 1, "verify parameters in " +
			 downloadPropertiesFilename );
			 

	    e.printStackTrace();
	}
    }

    // inner classes

    private static class ParamInfo_ {

	// data

	private static final String KEY_KEYSTORE_PATH_ = "keystorePath";
	private static final String KEY_KEYSTORE_PASSWORD_ = "keystorePassword";
	private static final String KEY_HOST_ = "host";
	private static final String KEY_PORT_ = "port";
	private static final String KEY_PATH_ = "path";
	private static final String KEY_DOWNLOAD_REQUEST_XML_FILENAME_ = "downloadRequestXMLFilename";
	private static final String KEY_XSLT_STYLESHEET_FILENAME_ = "xsltStylesheetFilename";

	private PropertyResourceBundle fResourceBundle_;

	// constructors

	public ParamInfo_( String aDownloadPropertiesFilename )
	    throws IOException {

	    InputStream is;

	    is =
		ClassLoader.getSystemResourceAsStream( aDownloadPropertiesFilename );

	    fResourceBundle_ =
		new PropertyResourceBundle( is );
	}

	public String getKeystorePath() {

	    return fResourceBundle_.getString( KEY_KEYSTORE_PATH_ );
	}

	public String getKeystorePassword() {

	    return fResourceBundle_.getString( KEY_KEYSTORE_PASSWORD_ );
	}

	public String getHost() {

	    return fResourceBundle_.getString( KEY_HOST_ );
	}

	public int getPort() {

	    int port;
	    String portStr;

	    try {

		portStr = fResourceBundle_.getString( KEY_PORT_ );
		
		port = ( portStr == null ) ? DEFAULT_SSL_LISTEN_PORT_ :
		                             Integer.parseInt( portStr );
	    }
	    catch( MissingResourceException e ) {

		port = DEFAULT_SSL_LISTEN_PORT_;
	    }

	    return port;
	}

	public String getPath() {

	    return fResourceBundle_.getString( KEY_PATH_ );
	}

	public String getDownloadRequestXML() throws IOException {

	    String filename;

	    filename =
		fResourceBundle_.getString( KEY_DOWNLOAD_REQUEST_XML_FILENAME_ );

	    return FileUtil.getFileContents( filename );
	}

	public String getXSLTStylesheet() throws IOException {

	    String filename;

	    try {

		filename =
		    fResourceBundle_.getString( KEY_XSLT_STYLESHEET_FILENAME_ );
	    }
	    catch( MissingResourceException e ) {

		return null;
	    }

	    return FileUtil.getFileContents( filename );
	}
    }
}
