#ifndef MYUNITYPLUGIN_H
#define MYUNITYPLUGIN_H

#include "Lib.h"

#ifdef __cplusplus
extern "C" {

#else // !__cplusplus

#endif // __cplusplus

MYUNITYPLUGIN_SYMBOL int initFoo(int f_new);
MYUNITYPLUGIN_SYMBOL int doFoo(int bar);
MYUNITYPLUGIN_SYMBOL int termFoo();

#ifdef __cplusplus
}
#else // !__cplusplus

#endif // __cplusplus
#endif // MYUNITYPLUGIN_H