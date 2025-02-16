import AdminAboutToStartComponent from '@/components/gameStateComponents/admin/AdminAboutToStartComponent';
import AdminFinishedComponent from '@/components/gameStateComponents/admin/AdminFinishedComponent';
import AdminInProgressComponent from '@/components/gameStateComponents/admin/AdminInProgressComponent';
import AdminRegistrationComponent from '@/components/gameStateComponents/admin/AdminRegistrationComponent';
import AdminWtfStateComponent from '@/components/gameStateComponents/admin/AdminWtfStateComponent';
import GameStateComponent from '@/components/GameStateComponent';
import Announcements from '@/components/AnnouncementsComponent';
import AddAnnouncementComponent from '@/components/AddAnnouncementComponent';

const AdminPage = () => {
	return (
		<div>
			<h1>Admin page</h1>
			<div>
				<p>Current game state</p>
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
				<AddAnnouncementComponent />
				<Announcements showDeleteButton={true} />
			</div>
		</div>
	);
};

export default AdminPage;
