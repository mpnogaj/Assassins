import RegistrationStatusDto from '@/types/dto/game/registrationStatusDto';
import { FetchableComponent, useFetchableComponent } from './hoc/FetchableComponent';
import { empty } from '@/types/other';
import { ReactNode } from 'react';
import { sendPost } from '@/utils/fetchUtils';
import Endpoints from '@/endpoints';

class RegistrationComponent extends FetchableComponent<RegistrationStatusDto, empty> {
	registerBtnHandler = async () => {
		await sendPost(Endpoints.game.register);
		this.props.refetch();
	};

	render(): ReactNode {
		return (
			<div>
				<h2>You {this.props.data.registered ? 'are' : 'are not'} registered</h2>
				<a className="btn btn-primary mt-3" onClick={() => this.registerBtnHandler()}>
					{!this.props.data.registered ? 'Register' : 'Unregister'}
				</a>
			</div>
		);
	}
}

export default useFetchableComponent(RegistrationComponent);
