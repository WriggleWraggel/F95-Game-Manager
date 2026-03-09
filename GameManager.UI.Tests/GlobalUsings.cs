global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;

global using Xunit;
global using FluentAssertions;
global using NSubstitute;
global using Bunit;

global using Fluxor;
global using Microsoft.AspNetCore.Components;

global using GameManager.Core.Data;
global using GameManager.Core.Data.F95;
global using GameManager.Core.Data.Settings;
global using GameManager.Core.MediatR.F95.Queries;

global using GameManager.UI.Features.GameLibrary;
global using GameManager.UI.Features.OnlineSearch;
global using GameManager.UI.Features.Settings;
global using GameManager.UI.Features.F95Login;
global using GameManager.UI.Features.GameUpdater;
global using GameManager.UI.Features.GameArchiveImporter;
global using GameManager.UI.Features.Modal;
