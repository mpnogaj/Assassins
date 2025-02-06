import AdminAboutToStartComponent from '@/components/admin/AdminAboutToStartComponent';
import AdminFinishedComponent from '@/components/admin/AdminFinishedComponent';
import AdminInProgressComponent from '@/components/admin/AdminInProgressComponent';
import AdminRegistrationComponent from '@/components/admin/AdminRegistrationComponent';
import AdminWtfStateComponent from '@/components/admin/AdminWtfStateComponent';
import GameStateComponent from '@/components/GameStateComponent';
import { fetchExtendedGameProgress } from '@/dataFetchers/adminFetchers';
import { fetchGameState } from '@/dataFetchers/gameFetchers';
import React from 'react';

class AdminPage extends React.Component {
	render() {
		return (
			<div>
				<h1>Admin page</h1>
				<div>
					<p>Current game state</p>
					<GameStateComponent
						dataFetcher={fetchGameState}
						RegistrationStateComponent={<AdminRegistrationComponent />}
						AboutToStartStateComponent={<AdminAboutToStartComponent />}
						InProgressStateComponent={
							<AdminInProgressComponent dataFetcher={fetchExtendedGameProgress} />
						}
						FinishedStateComponent={<AdminFinishedComponent />}
						UnknownStateComponent={
							<AdminWtfStateComponent GameStateComponentStateName="UnknownStateComponent" />
						}
						FallbackStateComponent={
							<AdminWtfStateComponent GameStateComponentStateName="FallbackStateComponent" />
						}
					/>
				</div>
			</div>
		);
	}
}

export default AdminPage;
