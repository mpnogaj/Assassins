import Endpoints from '@/endpoints';
import KillRequestDto from '@/types/dto/game/killRequestDto';
import { sendPost } from '@/utils/fetchUtils';
import { useState } from 'react';

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
		<div className="flex flex-col items-center">
			<h4 className="text-lg text-white mb-2">Twój cel: {props.targetName}</h4>
			<form
				className="flex flex-row items-center gap-2"
				onSubmit={e => {
					e.preventDefault();
					killPlayer();
				}}
			>
				<input
					className="p-2 border rounded-lg bg-gray-700 text-white"
					type="text"
					value={killBoxInput}
					onChange={e => setKillBoxInput(e.target.value)}
				></input>
				<button
					className="px-4 py-2 rounded-lg text-white font-medium transition-all 
                   bg-red-500 hover:bg-red-600 active:bg-red-700 focus:outline-none"
					type="submit"
				>
					Zabij
				</button>
			</form>
		</div>
	);
};

export default KillComponent;
