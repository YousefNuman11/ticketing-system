import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext.jsx';
import Input from '../../components/ui/Input.jsx';
import Button from '../../components/ui/Button.jsx';
import Alert from '../../components/ui/Alert.jsx';

export default function Register() {
  const { register } = useAuth();
  const navigate = useNavigate();
  const [form, setForm] = useState({
    fullName: '', email: '', password: '', mobileNumber: '', address: '',
  });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const set = (k) => (e) => setForm({ ...form, [k]: e.target.value });

  const submit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      await register(form);
      navigate('/', { replace: true });
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-50 px-4 py-10">
      <div className="w-full max-w-lg rounded-2xl border border-gray-200 bg-white p-8 shadow-theme-md">
        <div className="mb-6 text-center">
          <h1 className="text-2xl font-semibold text-gray-800">Create your account</h1>
          <p className="mt-1 text-sm text-gray-500">Register as a client to submit tickets</p>
        </div>
        <Alert>{error}</Alert>
        <form onSubmit={submit} className="grid grid-cols-1 gap-4 sm:grid-cols-2">
          <Input label="Full name" value={form.fullName} onChange={set('fullName')} required className="sm:col-span-2" />
          <Input label="Email" type="email" value={form.email} onChange={set('email')} required />
          <Input label="Mobile number" value={form.mobileNumber} onChange={set('mobileNumber')} required />
          <Input label="Address" value={form.address} onChange={set('address')} className="sm:col-span-2" />
          <Input label="Password" type="password" value={form.password} onChange={set('password')} required className="sm:col-span-2" />
          <Button type="submit" disabled={loading} className="w-full sm:col-span-2">
            {loading ? 'Creating...' : 'Create account'}
          </Button>
        </form>
        <p className="mt-6 text-center text-sm text-gray-500">
          Already have an account?{' '}
          <Link to="/login" className="font-medium text-brand-600 hover:underline">
            Sign in
          </Link>
        </p>
      </div>
    </div>
  );
}
