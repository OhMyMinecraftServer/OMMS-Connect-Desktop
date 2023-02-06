﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMMS.Core;

enum ResponseCode
{
    FAIL,
    OK,
    PERMISSION_DENIED,
    OPERATION_ALREADY_EXISTS,
    CONTROLLER_NOT_EXIST,
    CONTROLLER_NO_STATUS,
    NO_WHITELIST,
    WHITELIST_NOT_EXIST, RUNNER_NOT_EXIST,
    NO_SUCH_PLAYER,
    PLAYER_ALREADY_EXISTS,
    VERSION_NOT_MATCH,
    ANNOUNCEMENT_NOT_EXIST,
    INVALID_ARGUMENTS,
    NO_RUNNER, UNDEFINED
}