const PlayerAliveComponent = (props: { playerAlive: boolean }) => {
	const { playerAlive } = props;

	return (
		<h2 className={`text-xl font-semibold ${playerAlive ? 'text-green-500' : 'text-red-500'}`}>
			{playerAlive ? 'Dalej żyjesz - Walcz synu!' : 'Jesteś martwy - Przegrałeś synu...'}
		</h2>
	);
};

export default PlayerAliveComponent;
