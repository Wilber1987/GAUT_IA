# SNI_UNAN
@startuml
Bob->Alice : hello
@enduml


clonar con todos los submodulos: 
´´git clone --recurse-submodules -j8 https://github.com/Wilber1987/SIAC_CCA.git ´´

clonar solo sub modulos:
´´git submodule update --init --recursive´´
git submodule update --init --remote


configuracion de modulos

[submodule "UI/wwwroot/WDevCore"]
	path = UI/wwwroot/WDevCore
	url = https://github.com/Wilber1987/WDevCore
[submodule "CAPA_DATOS"]
	path = CAPA_DATOS
	url = https://github.com/Wilber1987/APPCORE.git

git submodule add  https://github.com/Wilber1987/WDevCore UI/wwwroot/WDevCore

git submodule add  https://github.com/Wilber1987/CSharpAttributes.git UI/CSharpAttributes

git submodule add  https://github.com/Wilber1987/AppCore.git 

git submodule update --remote
entonces

git commit && git push


push de submodule
git push origin HEAD:main

ixKQx9yhnazTxAdS4GGV2g==


https://localhost:7100/Questionnaires/ResolveForm?token=55343901-7872-4546-b25d-e31f528c7db9&id_test=1
//