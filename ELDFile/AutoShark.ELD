{ Helpful instructions on the use of EasyLanguage, such as this, appear below and are 
  contained within French curly braces {}.  There is no need to erase these 
  instructions when using EasyLanguage in order for it to function properly, because 
  this text will be ignored. }

{ STEP 1 OF 2: Replace <CRITERIA> with the criteria that will trigger a Buy at the open 
  of the next bar using a market order. }
Inputs:ID(1),Workspace("Scalper");
DEFINEDLLFUNC: "RegUtilLnk.dll",int,"GetMonitorPathLength";
DEFINEDLLFUNC: "RegUtilLnk.dll",int,"GETMONITORPATH",multiple;


  Variables: dirpath("C:\Program Files\Finance\Scalper\TSsignals\Export"),l1(0),l2(0),l3(0),ptr(0);
  Variables: CurState(0),CurContracts(0),OrderContracts(0),fn(""),dirname(" "), intrabarpersist numTick(0),intrabarpersist prevContracts(0);
  variables: newsymbol(" "),acct(" "),opnePosition(0); 



[IntrabarOrderGeneration=TRUE]  
	
	if currentbar=1 then begin
		l1=getmonitorpathlength;
	{	l1=l1; }
		
		dirname=leftstr(dirpath,l1);
	{		fileappend("c:\dirinfo.txt",dirname+newline);  }
		l2=GETMONITORPATH((lpstr)dirname,(int)l1);
	{
	 l3=strlen(dirname);
	 fileappend("c:\dirinfo.txt",numtostr(l1,0)+"   "+numtostr(l2,0)+"   "+numtostr(l3,0)+"   "+dirname+newline);
	}
	{	ID=random(298564);}
	end;
	CurContracts=0;
	CurState=MarketPosition;

	if Marketposition=-1 then  
		CurContracts=CurrentContracts* -1; 
	if MarketPosition=1 then
		CurContracts=CurrentContracts;


   
if lastbaronchart then begin
	if numtick=0 then 
		prevcontracts=CurContracts[1]
	else
		prevcontracts=prevcontracts;
	numtick=numtick+1;

	if Marketposition=-1 then  
		CurContracts=CurrentContracts* -1; 
	if MarketPosition=1 then
		CurContracts=CurrentContracts;
	newsymbol=getvalidfilename(getsymbolname);
	fn=dirname+newsymbol+numtostr(random(298564),0)+".req";
	
	acct= skipchar(workspace," "); 
    {   	
	fileappend("c:\dirinfo.txt","Vijay point1 symbol->" +getsymbolname +"prevcontracts-> " + numtostr(prevcontracts,0) + " CurContracts-> " + numtostr(CurContracts,0)+" MarketPosition->" +numtostr(MarketPosition,0) +NewLine);
	}

		OrderContracts=CurContracts-prevcontracts ; 
	{fileappend("c:\mydata.txt",newsymbol+newline);}
{fileappend("c:\mydata.txt",Numtostr(currentbar,0)+" - curstate:"+numtostr(curstate,0)+"  currentcontracts:"+numtostr(currentcontracts,0) +" prevcontract:" +numtostr(prevcontracts,0)+newline);}
	if ordercontracts<>0 then prevcontracts=CurContracts;
opnePosition = MarketPosition *  CurrentContracts;
If OrderContracts>0 then
	FileAppend(fn, NumToStr(Date, 0)+ " "+numtostr(time,0) + " "+getexchangename+ " "+getsymbolname+" Buy " + numtostr(orderContracts,0)+ " "+numtostr(id,0)+" "+acct+" "+numtostr(opnePosition,0)  +NewLine);

If OrderContracts<0 then
	FileAppend(fn, NumToStr(Date, 0)+ " "+numtostr(time,0) + " "+getexchangename+ " "+getsymbolname+ " Sell " + numtostr(pos(orderContracts),0)+" "+numtostr(id,0)+" "+acct+" "+numtostr(opnePosition,0) +NewLine);	

{ STEP 2 OF 2: Replace "Entry Name" (leaving the quotes) with a short name for the 
  entry.  The entry name will appear on the chart above/below the trade arrows and in 
  the trade by trade performance report. }
end;
