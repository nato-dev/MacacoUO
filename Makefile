MCS=mcs
EXENAME=ServUO
CURPATH=`pwd`
SRVPATH=${CURPATH}/Server
SDKPATH=${CURPATH}/Ultima
REFS=System.Drawing.dll,System.Web.dll,System.Data.dll
NOWARNS=0618,0219,0414,1635

PHONY : default build clean run

default: run

debug: 
	echo "Compilando Server.dll"
	mcs -target:library -out:`pwd`/Server.dll -r:`pwd`/Ultima.dll,Microsoft.CodeDom.Providers.DotNetCompilerPlatform.dll,${REFS} -nowarn:0618,0219,0414,1635 -d:DEBUG -d:MONO -d:ServUO -d:NEWTIMERS -optimize+ -nologo -debug -unsafe -recurse:`pwd`/Server/*.cs
	echo "Compilando Scripts.dll"
mcs -target:library -out:`pwd`/Scripts.dll -r:`pwd`/Ultima.dll,`pwd`/Server.dll,${REFS} -nowarn:0618,0219,0414,1635 -d:DEBUG -d:MONO -d:ServUO -d:NEWTIMERS -optimize+ -nologo -debug -unsafe -recurse:`pwd`/Scripts/*.cs

	${MCS} -target:library -out:${CURPATH}/Ultima.dll -r:${REFS} -nowarn:${NOWARNS} -d:DEBUG -d:MONO -d:ServUO -d:NEWTIMERS -optimize+ -nologo -debug -unsafe -recurse:${SDKPATH}/*.cs
	${MCS} -win32icon:${SRVPATH}/servuo.ico -r:${CURPATH}/Ultima.dll,${REFS} -nowarn:${NOWARNS} -target:exe -out:${CURPATH}/${EXENAME}.exe -d:DEBUG -d:MONO -d:ServUO -d:NEWTIMERS -nologo -debug -unsafe -recurse:${SRVPATH}/*.cs
	sed -i.bak -e 's/<!--//g; s/-->//g' ${EXENAME}.exe.config
	

mcs -target:library -out:`pwd`/Server.dll -r:`pwd`/Ultima.dll,System.Drawing.dll,Microsoft.CodeDom.Providers.DotNetCompilerPlatform.dll -nowarn:0618,0219,0414,1635 -d:DEBUG -d:MONO -d:ServUO -d:NEWTIMERS -optimize+ -nologo -debug -unsafe -recurse:`pwd`/Server/*.cs

mcs -target:library -out:"${CURPATH}/Scripts.dll" -r:"${CURPATH}/Ultima.dll","${CURPATH}/Server.dll",${REFS} -nowarn:${NOWARNS} -d:DEBUG ${DEFS} -nologo -debug -unsafe -recurse:"${SCRPATH}/*.cs"

mcs -target:library -out:`pwd`/Scripts.dll -r:`pwd`/Ultima.dll,`pwd`/Server.dll,System.Data.Dll,System.Drawing.dll,Microsoft.CodeDom.Providers.DotNetCompilerPlatform.dll -nowarn:0618,0219,0414,1635 -d:DEBUG -d:MONO -d:ServUO -d:NEWTIMERS -optimize+ -nologo -debug -unsafe -recurse:`pwd`/Scripts/*.cs

mcs -win32icon:`pwd`/Server/servuo.ico -r:`pwd`/Ultima.dll,System.Drawing.dll,Microsoft.CodeDom.Providers.DotNetCompilerPlatform.dll -nowarn:0618,0219,0414,1635 -target:exe -out:`pwd`/ServUO.exe -d:DEBUG -d:MONO -d:ServUO -d:NEWTIMERS -nologo -debug -unsafe -recurse:`pwd`/Server/*.cs


run: build
	${CURPATH}/${EXENAME}.sh

build: ${EXENAME}.sh

clean:
	rm -f ${EXENAME}.sh
	rm -f ${EXENAME}.exe
	rm -f ${EXENAME}.exe.mdb
	rm -f Ultima.dll
	rm -f Ultima.dll.mdb
	rm -f *.bin

Ultima.dll: Ultima/*.cs
	${MCS} -target:library -out:${CURPATH}/Ultima.dll -r:${REFS} -nowarn:${NOWARNS} -d:MONO -d:ServUO -d:NEWTIMERS -nologo -optimize -unsafe -recurse:${SDKPATH}/*.cs

${EXENAME}.exe: Ultima.dll Server/*.cs
	${MCS} -win32icon:${SRVPATH}/servuo.ico -r:${CURPATH}/Ultima.dll,${REFS} -nowarn:${NOWARNS} -target:exe -out:${CURPATH}/${EXENAME}.exe -d:MONO -d:ServUO -d:NEWTIMERS -nologo -optimize -unsafe -recurse:${SRVPATH}/*.cs

${EXENAME}.sh: ${EXENAME}.exe
	echo "#!/bin/sh" > ${CURPATH}/${EXENAME}.sh
	echo "mono ${CURPATH}/${EXENAME}.exe" >> ${CURPATH}/${EXENAME}.sh
	chmod a+x ${CURPATH}/${EXENAME}.sh
	sed -i.bak -e 's/<!--//g; s/-->//g' ${EXENAME}.exe.config
