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
		<div className="flex min-w-screen items-center justify-center bg-gray-900">
			<div className="w-full max-w-2xl rounded-lg bg-gray-800 p-5 shadow-lg">
				<h1 className="text-center text-gray-500 text-2xl font-bold">Admin page</h1>
				<div>
					<p className="text-center text-lg text-gray-400">Current game state</p>
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
		</div>
	);
};

export default AdminPage;
