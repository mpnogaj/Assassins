import FetchErrorComponent from '@/components/FetchErrorComponent';
import LoaderComponent from '@/components/LoaderComponent';
import { fetchGameProgress, fetchPlayerData } from '@/dataFetchers/gameFetchers';
import Endpoints from '@/endpoints';
import { useDataFetch } from '@/hooks/useDataFetch';
import KillRequestDto from '@/types/dto/game/killRequestDto';
import { sendPost } from '@/utils/fetchUtils';
import { QRCodeSVG } from 'qrcode.react';
import { useState } from 'react';
import { useSearchParams } from 'react-router';

const PlayerAliveComponent = (props: { playerAlive: boolean }) => {
	const { playerAlive } = props;

	return (
		<h2>{playerAlive ? 'Dalej żyjesz - Walcz synu!' : 'Jesteś martwy - Przegrałeś synu...'}</h2>
	);
};

const PlayerInfoComponent = (props: { alive: boolean; killCode: string }) => {
	const killUrl = `${window.location.origin}/?killCode=${props.killCode}`;

	return (
		<div>
			<PlayerAliveComponent playerAlive={props.alive} />
			<h4>Twój kod: {props.killCode}</h4>
			<QRCodeSVG value={killUrl} />
		</div>
	);
};

const KillComponent = (props: { targetName: string; initialTextBoxContent: string }) => {
	const [killBoxInput, setKillBoxInput] = useState(props.initialTextBoxContent);

	const killPlayer = async () => {
		const payload: KillRequestDto = {
			killCode: killBoxInput
		};

		try {
			await sendPost(Endpoints.game.kill, payload);
			alert('Kill successful');
		} catch {
			alert('Invalid kill code');
		}
	};

	return (
		<div>
			<h4>Twój cel: {props.targetName}</h4>
			<form
				onSubmit={e => {
					e.preventDefault();
					killPlayer();
				}}
			>
				<input
					type="text"
					value={killBoxInput}
					onChange={e => setKillBoxInput(e.target.value)}
				></input>
				<button type="submit">Zabij</button>
			</form>
		</div>
	);
};

const PlayerActionsComponent = () => {
	const { data, isLoading, isError, refetch } = useDataFetch(fetchPlayerData);
	const [searchParams, setSearchParams] = useSearchParams();

	const _1 = refetch;
	const _2 = setSearchParams;

	if (isLoading) {
		return <LoaderComponent />;
	}

	if (isError || data == undefined) {
		return null;
	}

	const maybeSearchParams = searchParams.get('killCode');

	return (
		<div>
			<PlayerInfoComponent alive={data.alive} killCode={data.killCode} />
			<KillComponent
				targetName={data.targetName}
				initialTextBoxContent={(maybeSearchParams as string) ?? ''}
			/>
		</div>
	);
};

const GameProgressComponent = () => {
	const { data, isLoading, isError, refetch } = useDataFetch(fetchGameProgress);
	const _ = refetch;

	if (isLoading) {
		return <LoaderComponent />;
	}

	if (isError || data == undefined) {
		return <FetchErrorComponent />;
	}

	return (
		<div>
			<h4>
				Pozostało graczy: {data.alivePlayers}/{data.totalPlayers}
			</h4>
		</div>
	);
};

const UserInProgressComponent = () => {
	return (
		<div>
			<GameProgressComponent />
			<PlayerActionsComponent />
		</div>
	);
};

export default UserInProgressComponent;
