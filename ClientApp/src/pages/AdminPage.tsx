import AdminAboutToStartComponent from '@/components/gameStateComponents/admin/AdminAboutToStartComponent';
import AdminFinishedComponent from '@/components/gameStateComponents/admin/AdminFinishedComponent';
import AdminInProgressComponent from '@/components/gameStateComponents/admin/AdminInProgressComponent';
import AdminRegistrationComponent from '@/components/gameStateComponents/admin/AdminRegistrationComponent';
import AdminWtfStateComponent from '@/components/gameStateComponents/admin/AdminWtfStateComponent';
import GameStateComponent from '@/components/GameStateComponent';
import React from 'react';
import { WebSocketProvider } from '@/components/WebSocketProvider';

const AdminPage = () => {
	return (
		<div>
			<h1>Admin page</h1>
			<div>
				<p>Current game state</p>
				<WebSocketProvider>
					<GameStateComponent
						RegistrationStateComponent={<AdminRegistrationComponent />}
						AboutToStartStateComponent={<AdminAboutToStartComponent />}
						InProgressStateComponent={<AdminInProgressComponent />}
						FinishedStateComponent={<AdminFinishedComponent />}
						UnknownStateComponent={
							<AdminWtfStateComponent GameStateComponentStateName="UnknownStateComponent" />
						}
						FallbackStateComponent={
							<AdminWtfStateComponent GameStateComponentStateName="FallbackStateComponent" />
						}
					/>
				</WebSocketProvider>
			</div>
		</div>
	);
};

export default AdminPage;
