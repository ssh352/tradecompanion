#define DLLF _declspec(dllexport)

DLLF void MedianPrice(float *high, float *low, float *outp, long numbars) 
{
  for (long i=0; i<numbars; i++)
    outp[i]=(high[i]+low[i])/2;
}

