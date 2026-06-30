import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext.jsx';
import { useTheme } from '../../context/ThemeContext.jsx';
import { useLanguage } from '../../context/LanguageContext.jsx';
import Input from '../../components/ui/Input.jsx';
import Button from '../../components/ui/Button.jsx';
import Alert from '../../components/ui/Alert.jsx';

export default function AuthPage() {
  const { login, register, user } = useAuth();
  const { colors } = useTheme();
  const { isRTL } = useLanguage();
  const navigate = useNavigate();

  const [isRegister, setIsRegister] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const [identifier, setIdentifier] = useState('');
  const [password, setPassword] = useState('');

  const [form, setForm] = useState({
    fullName: '',
    email: '',
    password: '',
    mobileNumber: '',
    address: '',
  });

  useEffect(() => {
    if (user) {
      navigate('/', { replace: true });
    }
  }, [user, navigate]);

  const setField = (key) => (e) => {
    setForm((prev) => ({
      ...prev,
      [key]: e.target.value,
    }));
  };

  const handleLogin = async (e) => {
    e.preventDefault();

    if (!identifier.trim() || !password.trim()) {
      setError('Please fill in all fields');
      return;
    }

    try {
      setLoading(true);
      setError('');
      await login(identifier, password);
      navigate('/', { replace: true });
    } catch (err) {
      setError(err.message || 'Login failed');
    } finally {
      setLoading(false);
    }
  };

  const handleRegister = async (e) => {
    e.preventDefault();

    if (
      !form.fullName.trim() ||
      !form.email.trim() ||
      !form.mobileNumber.trim() ||
      !form.password.trim()
    ) {
      setError('Please fill in all required fields');
      return;
    }

    try {
      setLoading(true);
      setError('');
      await register(form);
      navigate('/', { replace: true });
    } catch (err) {
      setError(err.message || 'Registration failed');
    } finally {
      setLoading(false);
    }
  };

  // In LTR: login sits at "start" (logical-left), register sits at "end" (logical-right).
  // In RTL: same logical slots, but physically mirrored - login at physical-right, register at physical-left.
  // We use logical offsets (insetInlineStart) instead of literal left/translateX so this
  // works correctly regardless of text direction.

  const panelBaseStyle = {
    position: 'absolute',
    top: 0,
    insetInlineStart: 0,
    height: '100%',
    width: '50%',
    padding: '2.5rem',
    transition: 'transform 0.7s ease-in-out, opacity 0.7s ease-in-out',
  };

  // translateX direction must flip under RTL since the browser doesn't
  // automatically mirror transform: translateX for logical positioning.
  const slideDistance = isRTL ? '-100%' : '100%';

  return (
    <div
      dir={isRTL ? 'rtl' : 'ltr'}
      style={{
        minHeight: '100vh',
        backgroundColor: colors.bgPage,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        padding: '0 16px',
      }}
    >
      <div
        style={{
          position: 'relative',
          width: '100%',
          maxWidth: 1024,
          minHeight: 600,
          overflow: 'hidden',
          borderRadius: 24,
          backgroundColor: colors.bgCard,
          boxShadow: '0 25px 60px rgba(0,0,0,0.18)',
        }}
        className="auth-card"
      >
        {/* LOGIN PANEL - always sits in the "start" slot, slides out of view when registering */}
        <div
          style={{
            ...panelBaseStyle,
            zIndex: isRegister ? 10 : 20,
            opacity: isRegister ? 0 : 1,
            transform: isRegister ? `translateX(${isRTL ? '100%' : '-100%'})` : 'translateX(0)',
          }}
        >
          <form onSubmit={handleLogin} style={{ height: '100%', display: 'flex', flexDirection: 'column', justifyContent: 'center' }}>
            <h1 style={{ fontSize: 32, fontWeight: 800, textAlign: 'center', marginBottom: 8, color: colors.textPrimary }}>
              Sign In
            </h1>
            <p style={{ textAlign: 'center', color: colors.textSecondary, marginBottom: 28 }}>
              Welcome back to the support desk
            </p>

            {error && <Alert>{error}</Alert>}

            <Input
              label="Email or Mobile Number"
              value={identifier}
              onChange={(e) => setIdentifier(e.target.value)}
              required
            />
            <Input
              label="Password"
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />

            <Button
              type="submit"
              disabled={loading || !identifier.trim() || !password.trim()}
              className="w-full mt-4"
            >
              {loading ? 'Signing In...' : 'Sign In'}
            </Button>
          </form>
        </div>

        {/* REGISTER PANEL - always sits in the "end" slot, slides into view when registering */}
        <div
          style={{
            ...panelBaseStyle,
            insetInlineStart: 'auto',
            insetInlineEnd: 0,
            zIndex: isRegister ? 20 : 10,
            opacity: isRegister ? 1 : 0,
            transform: isRegister ? 'translateX(0)' : `translateX(${slideDistance})`,
          }}
        >
          <form onSubmit={handleRegister} style={{ height: '100%', display: 'flex', flexDirection: 'column', justifyContent: 'center', overflowY: 'auto' }}>
            <h1 style={{ fontSize: 32, fontWeight: 800, textAlign: 'center', marginBottom: 8, color: colors.textPrimary }}>
              Create Account
            </h1>
            <p style={{ textAlign: 'center', color: colors.textSecondary, marginBottom: 20 }}>
              Register as a client
            </p>

            {error && <Alert>{error}</Alert>}

            <Input label="Full Name" value={form.fullName} onChange={setField('fullName')} required />
            <Input label="Email" type="email" value={form.email} onChange={setField('email')} required />
            <Input label="Mobile Number" value={form.mobileNumber} onChange={setField('mobileNumber')} required />
            <Input label="Address" value={form.address} onChange={setField('address')} />
            <Input label="Password" type="password" value={form.password} onChange={setField('password')} required />

            <Button type="submit" disabled={loading} className="w-full mt-4">
              {loading ? 'Creating...' : 'Create Account'}
            </Button>
          </form>
        </div>

        {/* OVERLAY - sits in the "end" slot when login is active, slides to "start" slot when register is active */}
        <div
          style={{
            position: 'absolute',
            top: 0,
            height: '100%',
            width: '50%',
            zIndex: 30,
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            color: '#fff',
            background: 'linear-gradient(135deg, #4f46e5 0%, #9333ea 100%)',
            transition: 'transform 0.7s ease-in-out, border-radius 0.7s ease-in-out',
            insetInlineStart: 0,
            transform: isRegister
              ? 'translateX(0)'
              : `translateX(${slideDistance})`,
            borderRadius: isRegister
              ? (isRTL ? '0% 50% 50% 0% / 0% 100% 100% 0%' : '50% 0% 0% 50% / 100% 0% 0% 100%')
              : (isRTL ? '50% 0% 0% 50% / 100% 0% 0% 100%' : '0% 50% 50% 0% / 0% 100% 100% 0%'),
          }}
        >
          <div style={{ textAlign: 'center', padding: '0 40px' }}>
            {isRegister ? (
              <>
                <h1 style={{ fontSize: 32, fontWeight: 800, marginBottom: 16 }}>Welcome Back!</h1>
                <p style={{ marginBottom: 24 }}>Already have an account?</p>
                <button
                  type="button"
                  onClick={() => {
                    setError('');
                    setIsRegister(false);
                  }}
                  style={{
                    border: '1px solid #fff',
                    padding: '12px 32px',
                    borderRadius: 999,
                    fontWeight: 600,
                    background: 'transparent',
                    color: '#fff',
                    cursor: 'pointer',
                    transition: 'background-color 0.2s, color 0.2s',
                  }}
                  onMouseEnter={(e) => { e.currentTarget.style.backgroundColor = '#fff'; e.currentTarget.style.color = '#4f46e5'; }}
                  onMouseLeave={(e) => { e.currentTarget.style.backgroundColor = 'transparent'; e.currentTarget.style.color = '#fff'; }}
                >
                  Sign In
                </button>
              </>
            ) : (
              <>
                <h1 style={{ fontSize: 32, fontWeight: 800, marginBottom: 16 }}>Hello Friend!</h1>
                <p style={{ marginBottom: 24 }}>Create an account to submit tickets</p>
                <button
                  type="button"
                  onClick={() => {
                    setError('');
                    setIsRegister(true);
                  }}
                  style={{
                    border: '1px solid #fff',
                    padding: '12px 32px',
                    borderRadius: 999,
                    fontWeight: 600,
                    background: 'transparent',
                    color: '#fff',
                    cursor: 'pointer',
                    transition: 'background-color 0.2s, color 0.2s',
                  }}
                  onMouseEnter={(e) => { e.currentTarget.style.backgroundColor = '#fff'; e.currentTarget.style.color = '#4f46e5'; }}
                  onMouseLeave={(e) => { e.currentTarget.style.backgroundColor = 'transparent'; e.currentTarget.style.color = '#fff'; }}
                >
                  Sign Up
                </button>
              </>
            )}
          </div>
        </div>
      </div>

      <style>{`
        @media (max-width: 768px) {
          .auth-card {
            min-height: auto !important;
          }
        }
      `}</style>
    </div>
  );
}