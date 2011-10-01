package fxintegrate.util;

import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.io.InputStream;

public class FileUtil {

    /**
     * Returns contents <code>aFilename</code>.
     */
    public static String getFileContents( String aFilename )
	throws IOException {

	BufferedReader reader;
	String line;
	StringBuffer strBuf;

	reader = new BufferedReader( new FileReader ( aFilename ) );

	strBuf = new StringBuffer();

	while( ( line = reader.readLine() ) != null ) {

	    strBuf.append( line );
	    strBuf.append( '\n' );
	}

	reader.close();

	return strBuf.toString();
    }

    /**
     * Returns contents of <code>aFilename</code>, a file which resides in
     * the system classpath, as a <code>String</code>.
     */
    public static String getFileRelativeClassPathContents( String aFilename )
	throws IOException {

	byte byteArr[];
	int numBytes;
        InputStream is;

	if( ( is = ClassLoader.getSystemResourceAsStream( aFilename ) ) ==
	    null ) {

	    return null;
	}

	numBytes = is.available();

	byteArr = new byte[ numBytes ];

	is.read( byteArr, 0, numBytes );

	return new String( byteArr );
    }
}
