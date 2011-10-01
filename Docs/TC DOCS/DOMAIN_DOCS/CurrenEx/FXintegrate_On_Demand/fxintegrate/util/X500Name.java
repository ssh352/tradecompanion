package fxintegrate.util;

import java.text.ParseException;
import java.util.Enumeration;
import java.util.Hashtable;
import java.util.Iterator;
import java.util.List;
import java.util.Vector;

public class X500Name {

    public final static String CN = "CN";
    public final static String C  = "C";
    public final static String EMAIL = "Email";
    public final static String L  = "L";
    public final static String O  = "O";
    public final static String OU = "OU";
    public final static String ST = "ST";

    private Hashtable fRDNInfo_;

    private List fRDNSetOrder_;

    public X500Name() {

	fRDNInfo_ = new Hashtable();

	fRDNSetOrder_ = new Vector();
    }

    public X500Name( String aName ) throws ParseException {

	this();

	boolean firstRDN;
	boolean quoting;
	char c;
	int end;
	int i;
	int len;
	int start;

	len = aName.length();

	quoting = false;
	firstRDN = true;

	start = 0;
	end = -1;

	for( i = 0; i < len; i++ ) {

	    c = aName.charAt( i );

	    if( c == '"' ) {

		quoting = ! quoting;
	    }

	    if( ! quoting ) {

		if( i == ( len - 1 ) ) { // last RDN; this case first, just in
		                         // case there's only one RDN

		    parseRDN_( aName.substring( start, len ) );
		}
		else if( c == '=' ) { // non-last RDN

		    if( firstRDN ) {

			firstRDN = false;

			continue;
		    }

		    parseRDN_( aName.substring( start, end ) );

		    start = end + 1;
		}
		else if( c == ',' ) {

		    end = i;
		}
	    }
	    else if( i == ( len - 1 ) ) { // quoting, but reached end of aName

		throw new ParseException( "aName: " + aName, i );
	    }
	}
    }

    private void parseRDN_( String aRDNStr ) throws ParseException {

	int i;
	int len;
	String rdnName;
	String rdnVal;

	i = aRDNStr.indexOf( '=' );

	rdnName = aRDNStr.substring( 0, i ).trim();

	len = aRDNStr.length();

	rdnVal = aRDNStr.substring( i + 1, len ).trim();

	if( ( len = rdnVal.length() ) > 0 ) {

	    if( rdnVal.charAt( 0 ) == '"' ) { // strip quotes

		if( ( len == 1 ) || ( rdnVal.charAt( len - 1 ) != '"' ) ) {

		    throw new ParseException( "aRDNStr: " + aRDNStr +
					      ", rdnName: " + rdnName +
					      ", rdnVal: " + rdnVal, len - 1 );
		}

		rdnVal = rdnVal.substring( 1, len - 1 );
	    }
	}

	addRDN_( rdnName, rdnVal );
    }

    // add entries to fRDNInfo_ only via addRDN_() and setRDNValList_() b/c
    // these keep track of the order in which entries are added; this ordering
    // is used by toString()

    private void addRDN_( String aRDNName, String aRDNVal ) {

	List rdnValList;

	aRDNName = aRDNName.toLowerCase();

	if( ( rdnValList = ( List ) fRDNInfo_.get( aRDNName ) ) == null ) {

	    rdnValList = new Vector();
	}

	rdnValList.add( aRDNVal );

	fRDNInfo_.put( aRDNName, rdnValList );

	trackRDNOrder_( aRDNName );
    }

    private void setRDNValList_( String aRDNName, List aRDNValList ) {

	aRDNName = aRDNName.toLowerCase();

	fRDNInfo_.put( aRDNName, aRDNValList );

	trackRDNOrder_( aRDNName );
    }

    private void trackRDNOrder_( String aRDNName ) {

	aRDNName = aRDNName.toLowerCase();

	if( ! fRDNSetOrder_.contains( aRDNName ) ) {

	    fRDNSetOrder_.add( aRDNName );
	}
    }

    public String toString() {

	int len;
	Iterator rdnNames;
	Iterator rdnVals;
	List rdnValList;
	String rdnName;
	String rdnVal;
	StringBuffer strBuf;

	strBuf = new StringBuffer();

	for( rdnNames = fRDNSetOrder_.iterator(); rdnNames.hasNext(); ) {

	    rdnName = ( String ) rdnNames.next();

	    rdnName = rdnName.toLowerCase();

	    rdnValList = ( List ) fRDNInfo_.get( rdnName );

	    for( rdnVals = rdnValList.iterator(); rdnVals.hasNext(); ) {

		rdnVal = ( String ) rdnVals.next();

		strBuf.append( rdnName ).append( '=' ).append( '"' ).append( rdnVal ).append( '"' ).append( ", " );
	    }
	}

	len = strBuf.length();

	strBuf.setLength( len - 2 );

	return strBuf.toString();
    }

    private void removeRDN_( String aRDNName ) {

	aRDNName = aRDNName.toLowerCase();

	fRDNInfo_.remove( aRDNName );
    }

    /**
     * Returns a value associated with <code>aRDNName</code> ( case-insensitive
     * ), or <code>null</code> if no such value exists. If there are more than
     * one values associated with a given <code>aRDNName</code>, there are no
     * guarantees concerning which value will be returned, and it cannot be
     * assumed that this method will return the same value for every call; in
     * this case, it is best to use <code>getRDNValList()</code>.
     */
    public String getRDNVal( String aRDNName ) {

	List rdnValList;
	String val;

	rdnValList = getRDNValList( aRDNName );

	val  = ( ( rdnValList != null ) && ( rdnValList.size() > 0 ) ) ?
	    ( String ) rdnValList.get( 0 ) : null;

	return val;
    }

    /**
     * Sets the value of <code>aRDNName</code> to <code>aRDNVal</code>;
     * previously set value(s) are lost.
     *
     * To set multiple values, use <code>setRDNValList()</code>.
     */
    public void setRDNVal( String aRDNName, String aRDNVal ) {

	removeRDN_( aRDNName );

	addRDN_( aRDNName, aRDNVal );
    }

    /**
     * Returns a list containing values associated with <code>aRDNName</code> (
     * case-insensitive ), or <code>null</code> if no such values exist.
     */
    public List getRDNValList( String aRDNName ) {

	aRDNName = aRDNName.toLowerCase();

	return ( List ) fRDNInfo_.get( aRDNName );
    }

    /**
     * Sets the values of <code>aRDNName</code> to those contained in
     * <code>aRDNValList</code>; previously set value(s) are lost.
     */
    public void setRDNValList( String aRDNName, List aRDNValList ) {

	setRDNValList_( aRDNName, aRDNValList );
    }

    public String getCommonName() {

	return getRDNVal( CN );
    }

    public String getCountryName() {

	return getRDNVal( C );
    }

    public String getEmail() {

	return getRDNVal( EMAIL );
    }

    public String getLocalityName() {

	return getRDNVal( L );
    }

    public String getOrganizationName() {

	return getRDNVal( O );
    }

    public String getOrgUnitName() {

	return getRDNVal( OU );
    }

    public String getStateName() {

	return getRDNVal( ST );
    }

    public void dump() {

	Enumeration keys;
	Iterator vals;
	List list;
	String name;
	String val;

	println_( "dump(): " );

	for( keys = fRDNInfo_.keys(); keys.hasMoreElements(); ) {

	    name = ( String ) keys.nextElement();

	    list = ( List ) fRDNInfo_.get( name );

	    println_( "    name: " + name );

	    for( vals = list.iterator(); vals.hasNext(); ) {

		val = ( String ) vals.next();

		println_( "        val: " + val );
	    }
	}
    }

    private void println_( String aStr ) {

	System.out.println( aStr );
    }
}
