using Assassins.Web.Models;

namespace Assassins.Web.Services.GameService;

public abstract record GameState(string Name);

public record UnknownState() : GameState(Name: "unknown");
public record RegistrationState() : GameState("registration");
public record AboutToStartState(List<User> RegisteredUsers) : GameState("aboutToStart");
public record InProgressState(int AlivePlayers, int TotalPlayers) : GameState("inProgress");
public record FinishedState(User Winner) : GameState("finished");
