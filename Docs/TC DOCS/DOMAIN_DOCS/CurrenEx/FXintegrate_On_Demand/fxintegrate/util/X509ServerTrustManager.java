package fxintegrate.util;

/**
 * Title:        X509ServerTrustManager
 * Copyright:    Copyright (c) 2001
 * Company:      Currenex, Inc.
 * @author       Hu Liang
 * @version 1.0
 */

import java.io.FileInputStream;
import java.io.FileNotFoundException;

import java.util.Enumeration;
import java.util.Vector;

import java.security.KeyStore;
import java.security.KeyStoreException;
import java.security.cert.CertificateException;
import java.security.cert.X509Certificate;
import java.security.KeyStoreException;
import java.security.NoSuchAlgorithmException;
import java.security.InvalidKeyException;
import java.security.NoSuchProviderException;
import java.security.SignatureException;
import java.security.cert.CertificateException;

/**
 * This class is used by an HTTPS client to decide on which server certificates
 * to trust.  It is created by construction with a decoded(loaded) KeyStore.  It
 * will trust all aliases beginning with "TrustedCA", so all KeyStore aliases of
 * TrustCA* will be trusted.
 *
 * Since this class should be used as a server trust manager only.  All calls to
 * the isClientTrusted method will return false.
 */
public class X509ServerTrustManager implements com.sun.net.ssl.X509TrustManager {
    private final static String TRUST_ALIAS = "trustedca";
//    private final static String TRUST_ALIAS = "test";

    private X509Certificate[] fCerts = null;
    private KeyStore fKeyStore = null;

    public X509ServerTrustManager(KeyStore aKeystore) throws KeyStoreException {
        if (aKeystore == null) {
            new NullPointerException("X509ServerTrustManager received a null KeyStore.");
        }
        fKeyStore = aKeystore;

        // get trusted CA certs
        Enumeration enum = aKeystore.aliases();
        Vector v = new Vector();
        while (enum.hasMoreElements()) {
            String alias = (String)enum.nextElement();
            if (alias.toLowerCase().startsWith(TRUST_ALIAS)) {
                X509Certificate cert = (X509Certificate)aKeystore.getCertificate(alias);
                if (cert != null) {
                    v.addElement(cert);
                }
            }
        }

        fCerts = new X509Certificate[v.size()];
        v.copyInto(fCerts);
    }

    /**
     * Returns an array of trusted CA certificates based on the KeyStore used
     * to instantiate this X509ServerTrustManager.
     *
     * @return X509Certificate[] an array of trusted CA certificates
     */
    public X509Certificate[] getAcceptedIssuers() {
        return fCerts;
    }

    /**
     * This implementation of X509TrustManager is for validating server
     * certificates.  It is not intended to provide trust for clients.  As a
     * result, the isClientTrusted method will always return false.
     *
     * @param certs an array of X509Certificates
     * @return boolean always returns false
     */
    public boolean isClientTrusted(X509Certificate[] certs) {
        return false;
    }

    /**
     * Returns whether the certificate chain can be trusted based on the
     * KeyStore used to construct the object.
     *
     * @param certs an array of X509Certificates
     * @return boolean whether the cert chain can be trusted based on the
     * KeyStore used to construct the X509ServerTrustManager
     */
    public boolean isServerTrusted(X509Certificate[] certs) {
        for (int i=0; i<certs.length; i++) {
            for (int j=0; j<fCerts.length; j++) {
//                System.out.println("Comparing principals:");
//                System.out.println("  " + certs[i].getIssuerDN().toString());
//                try {
//                    System.out.println(new X500Name(certs[i].getIssuerDN().toString()).toString());
//                    System.out.println(new X500Name(fCerts[j].getSubjectDN().toString()).toString());
//                } catch (java.text.ParseException e) {
//                    System.out.println("ParseException!");
//                }
//                System.out.println("  " + fCerts[j].getSubjectDN().toString());

                String userCert = "";
                String caCert = "";
                try {
                    userCert = new X500Name(certs[i].getIssuerDN().toString()).toString();
                    caCert = new X500Name(fCerts[j].getSubjectDN().toString()).toString();
                } catch (java.text.ParseException e) {
                    return false;
                }

//                if (certs[i].getIssuerDN().equals(fCerts[j].getSubjectDN())) {
                if (userCert.equals(caCert)) {
//                    System.out.println("Found a trusted CA that matches the signer");
                    // we now have a matching issuer, verify the signature
                    try {
                        certs[i].verify(fCerts[j].getPublicKey());
                    // if the verification process throws any exceptions, return
                    // false
                    } catch (NoSuchAlgorithmException e) {
                        return false;
                    } catch (InvalidKeyException e) {
                        return false;
                    } catch (NoSuchProviderException e) {
                        return false;
                    } catch (SignatureException e) {
                        return false;
                    } catch (CertificateException e) {
                        return false;
                    } catch (Exception e) {
                        return false;
                    }
                    return true;
                }
            }
        }
        return false;
    }

    /**
     * Given a certificate chain, this method will walk through the chain from
     * the zeroth element until it finds a certificate where the issuer DN
     * matches that of the subject DN, which is considered a self signed root
     * certificate.
     *
     * @param certs an array of X509Certificates
     * @return X509Certificate the first self signed X509Certificate
     */
    private X509Certificate getRootCertificate_(X509Certificate[] certs) {
        for (int i=0; i<certs.length; i++) {
            // if the subject matches the issuer than it's a self signed cert,
            // which means it's a root certificate
            if (certs[i].getSubjectDN().equals(certs[i].getIssuerDN())) {
                return certs[i];
            }
        }

        // we didn't find a root certificate
        return null;
    }

    /**
     * Check to see if the given root certificate is in the list of trusted
     * CA certificates within this TrustManager.
     *
     * @param cert a root certificate
     * @return boolean whether the input root certificates is one of the trusted
     * root certificates
     */
    private boolean contains(X509Certificate cert) {
        for (int i=0; i<fCerts.length; i++) {
            if (cert.getSerialNumber().equals(fCerts[i].getSerialNumber())) {
                return true;
            }
        }
        return false;
    }

}
