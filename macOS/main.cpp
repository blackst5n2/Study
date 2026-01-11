#include "src/Core/AppInstance.h"
#include <iostream>

using namespace Core;

int main(int argc, char** argv){
    auto& app = Core::AppInstance::Get();

    if (app.Init()) {
        app.Run();
    }

    return 0;
}
