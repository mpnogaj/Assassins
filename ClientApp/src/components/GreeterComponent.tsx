import { FetchableComponent, useFetchableComponent } from './hoc/FetchableComponent';
import { UserDto } from '@/types/dto/userDto';

class GreeterComponent extends FetchableComponent<UserDto> {
	render() {
		return (
			<div>
				<h1>Hej {this.props.data.fullName} - Walcz synu!</h1>
			</div>
		);
	}
}

export default useFetchableComponent<UserDto>(GreeterComponent);
