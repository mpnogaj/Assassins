export interface GRecaptcha {
	ready: (callback: () => void) => void;
	execute: (sitekey: string, options: { action: string }) => Promise<string>;
}

declare global {
	interface Window {
		grecaptcha: {
			execute: (siteKey: string, options: { action: string }) => Promise<string>;
		};
	}
}

export const RECAPTCHA_SCRIPT_ID = 'grecaptcha-v3';
export const SITE_KEY = '6Levu9IqAAAAAC1o2GWAjj4HdBTiPKiW8fw8ypwb';
export const ACTION = 'submit';
