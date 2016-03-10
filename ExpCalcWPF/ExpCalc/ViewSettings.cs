using System;
using System.Windows.Media;
using System.Windows.Controls;

namespace ExpCalc
{
	/// <summary>
	/// ViewSettings �̊T�v�̐����ł��B
	/// </summary>
	public class ViewSettings
	{
		// ����̈�
		private MVColor c_body_bg;      // �w�i�F
		private MVColor c_body_text;    // �e�L�X�g�̐F
		private MVColor c_button;       // �{�^���̐F
		private MVColor c_button_text;  // �{�^���̃e�L�X�g�̐F
		private MVColor c_textbox;      // �e�L�X�g�{�b�N�X�̐F
		private MVColor c_text_text;    // �e�L�X�g�{�b�N�X�̃e�L�X�g�̐F

		// �v�Z�̈�
		private MVColor c_exp_area_bg;  // ���̗̈�̔w�i�F
		private MVColor c_exp_nonsel;   // �I������Ă��Ȃ��Ƃ��̎��̔w�i�F
		private MVColor c_exp_sel;      // �I�����ꂽ�Ƃ��̎��̔w�i�F
		private MVColor c_exp_boundary; // ���̘g�̐F
		private MVColor c_exp_text;     // ���̕����̐F

		// �t�H���g
		private XFont font_main;                      // �t�H�[���̃t�H���g
		private XFont font_calc;                      // �v�Z�̈�̃t�H���g

		/// <summary>
		/// �|���Z�̋L�����u���v�A����Z�̋L�����u�^�v�ŕ\�����郂�[�h
		/// </summary>
		private bool asc_mode = false;

		/// <summary>
		/// �C���[�W�̐ݒ�
		/// </summary>
		private ImageSettings image_settings;

		/// <summary>
		/// �C���[�W���g�����ǂ���
		/// </summary>
		private bool use_image = false;

		public ViewSettings(XFont main_font)
		{
			c_body_bg = MVColor.FromArgb(220, 240, 255);  // �w�i�F
			c_body_text = MVColor.FromArgb(0, 0, 0);      // �e�L�X�g�̐F
			c_button = MVColor.FromArgb(200, 220, 255);   // �{�^���̐F
			c_button_text = MVColor.FromArgb(0, 0, 0);    // �{�^���̃e�L�X�g�̐F
			c_textbox = MVColor.FromArgb(255, 245, 245);  // �e�L�X�g�{�b�N�X�̐F
			c_text_text = MVColor.FromArgb(0, 0, 0);      // �e�L�X�g�{�b�N�X�̃e�L�X�g�̐F

			c_exp_area_bg = MVColor.FromArgb(255, 245, 245);  // ���̗̈�̔w�i�F
			c_exp_nonsel = MVColor.FromArgb(220, 255, 255);   // �I������Ă��Ȃ��Ƃ��̎��̔w�i�F
			c_exp_sel = MVColor.FromArgb(200, 250, 250);      // �I�����ꂽ�Ƃ��̎��̔w�i�F
			c_exp_boundary = MVColor.FromArgb(150, 200, 200); // ���̘g�̐F
			c_exp_text = MVColor.FromArgb(0, 0, 0);           // ���̕����̐F

			font_main = main_font;
			font_calc = main_font; // �v�Z�̈�̃t�H���g�͉��Ƀ��C���E�B���h�E�̃t�H���g��ݒ肵�Ă���

			asc_mode = false;

			image_settings = new ImageSettings();
			use_image = false;
		}

        public ViewSettings(ViewSettings view_settings)
        {
            c_body_bg = view_settings.c_body_bg;             // �w�i�F
            c_body_text = view_settings.c_body_text;         // �e�L�X�g�̐F
            c_button = view_settings.c_button;               // �{�^���̐F
            c_button_text = view_settings.c_button_text;     // �{�^���̃e�L�X�g�̐F
            c_textbox = view_settings.c_textbox;             // �e�L�X�g�{�b�N�X�̐F
            c_text_text = view_settings.c_text_text;         // �e�L�X�g�{�b�N�X�̃e�L�X�g�̐F

            c_exp_area_bg = view_settings.c_exp_area_bg;     // ���̗̈�̔w�i�F
            c_exp_nonsel = view_settings.c_exp_nonsel;       // �I������Ă��Ȃ��Ƃ��̎��̔w�i�F
            c_exp_sel = view_settings.c_exp_sel;             // �I�����ꂽ�Ƃ��̎��̔w�i�F
            c_exp_boundary = view_settings.c_exp_boundary;   // ���̘g�̐F
            c_exp_text = view_settings.c_exp_text;           // ���̕����̐F

            font_main = view_settings.font_main;             // ���C���E�B���h�E�̃t�H���g
            font_calc = view_settings.font_calc;             // �v�Z�̈�̃t�H���g

            asc_mode = view_settings.asc_mode;

            image_settings = view_settings.image_settings;
            use_image = view_settings.use_image;
        }

        /// <summary>
        /// �w�i�F
        /// </summary>
        public MVColor BodyBackColor
		{
			get
			{
				return c_body_bg;
			}
			set
			{
				c_body_bg = value;
			}
		}

		/// <summary>
		/// �e�L�X�g�̐F
		/// </summary>
		public MVColor BodyTextColor
		{
			get
			{
				return c_body_text;
			}
			set
			{
				c_body_text = value;
			}
		}

		/// <summary>
		/// �{�^���̐F
		/// </summary>
		public MVColor ButtonBackColor
		{
			get
			{
				return c_button;
			}
			set
			{
				c_button = value;
			}
		}

		/// <summary>
		/// �{�^���̕����̐F
		/// </summary>
		public MVColor ButtonTextColor
		{
			get
			{
				return c_button_text;
			}
			set
			{
				c_button_text = value;
			}
		}

		/// <summary>
		/// �e�L�X�g�{�b�N�X�̐F
		/// </summary>
		public MVColor TextBackColor
		{
			get
			{
				return c_textbox;
			}
			set
			{
				c_textbox = value;
			}
		}

		/// <summary>
		/// �e�L�X�g�{�b�N�X�̃e�L�X�g�̐F
		/// </summary>
		public MVColor TextTextColor
		{
			get
			{
				return c_text_text;
			}
			set
			{
				c_text_text = value;
			}
		}

		/// <summary>
		/// �v�Z�̈�̕����̐F
		/// </summary>
		public MVColor CalcAreaTextColor
		{
			get
			{
				return c_exp_text;
			}
			set
			{
				c_exp_text = value;
			}
		}

		/// <summary>
		/// �v�Z�̈�̔w�i�F
		/// </summary>
		public MVColor CalcAreaBackColor
		{
			get
			{
				return c_exp_area_bg;
			}
			set
			{
				c_exp_area_bg = value;
			}
		}

		/// <summary>
		/// �v�Z�̈�̘g�̐F
		/// </summary>
		public MVColor CalcAreaFrameColor
		{
			get
			{
				return c_exp_boundary;
			}
			set
			{
				c_exp_boundary = value;
			}
		}

		/// <summary>
		/// �v�Z�̈�̘g�̒��̐F(�I������Ă��Ȃ�)
		/// </summary>
		public MVColor CalcAreaFrameBackColor
		{
			get
			{
				return c_exp_nonsel;
			}
			set
			{
				c_exp_nonsel = value;
			}
		}

		/// <summary>
		/// �v�Z�̈�̘g�̒��̐F(�I������Ă���)
		/// </summary>
		public MVColor CalcAreaFrameSelectedBackColor
		{
			get
			{
				return c_exp_sel;
			}
			set
			{
				c_exp_sel = value;
			}
		}

		/// <summary>
		/// �|���Z�̋L�����u���v�A����Z�̋L�����u�^�v�ŕ\�����郂�[�h
		/// </summary>
		public bool AsciiMode
		{
			get
			{
				return asc_mode;
			}
			set
			{
				asc_mode = value;
			}
		}
		
		///// <summary>
		///// �{�^���ɐF��ݒ肷��
		///// </summary>
		///// <param name="button">�{�^��</param>
		//public void SetButtonColors(System.Windows.Forms.Button button)
		//{
		//	button.BackColor = c_button;
		//	button.ForeColor = c_button_text;
		//}

		///// <summary>
		///// �e�L�X�g�{�b�N�X�ɐF��ݒ肷��
		///// </summary>
		///// <param name="text">�e�L�X�g�{�b�N�X</param>
		//public void SetTextBoxColors(System.Windows.Forms.TextBox text)
		//{
		//	text.BackColor = c_textbox;
		//	text.ForeColor = c_text_text;
		//}

		///// <summary>
		///// �`�F�b�N�{�b�N�X�ɐF��ݒ肷��
		///// </summary>
		///// <param name="check">�`�F�b�N�{�b�N�X</param>
		//public void SetCheckBoxColors(System.Windows.Forms.CheckBox check)
		//{
		//	check.ForeColor = c_text_text;
		//}

		/// <summary>
		/// ���C���E�B���h�E�̃t�H���g
		/// </summary>
		public XFont MainFont
		{
			get
			{
				return font_main;
			}
			set
			{
				font_main = value;
			}
		}

		/// <summary>
		/// �v�Z�̈�̃t�H���g
		/// </summary>
		public XFont CalcAreaFont
		{
			get
			{
				return font_calc;
			}
			set
			{
				font_calc = value;
			}
		}

		/// <summary>
		/// �C���[�W�̐ݒ�
		/// </summary>
		public ImageSettings ImageSettings
		{
			get
			{
				return image_settings;
			}
			set
			{
				image_settings = value;
			}
		}

		/// <summary>
		/// �C���[�W���g�����ǂ���
		/// </summary>
		public bool UseImage
		{
			get
			{
				return use_image;
			}
			set
			{
				use_image = value;
			}
		}
	}
}
